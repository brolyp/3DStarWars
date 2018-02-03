using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class enemyController : MonoBehaviour, IDamageable, IKillable, IEnemyNotification {

	public enum AIState {patrol, pursue, attack, search};
    public Transform[] patrolPoints;
    //public enemyPerceptionTrigger ePT;
    public int destPoint;
    NavMeshAgent agent;
	public float aimFuzz;
	public float aimRangeBottom;
	public float aimRangeTop;
	public float saveTime;
	public GameObject bulletPrefab;
	public Transform bulletSpawnT;
	public Transform rifle;
	int numShot;
	int numHit;
	RaycastHit outHit;
	Collider[] sphereHit;
	public AIState curAIState;
	public bool alertedToPlayer;
	public bool playerInRange;
	public Transform playerTransf;
	IEnemyNotification enemyNote;
	bool startedTimer;
	public Vector3 lastKnownPos;
	public float searchTimer;

	// Use this for initialization
	void Start () {
       // ePT = GetComponentInChildren<enemyPerceptionTrigger>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        destPoint = 0;
		aimFuzz = 15.0f;
		aimRangeTop = 1.5f;
		aimRangeBottom = -1.5f;
		saveTime = 0.0f;
        NextPoint();
		numShot = 0;
		numHit = 0;
		alertedToPlayer = false;
	}
	
	// Update is called once per frame
	void Update () {
		switch (curAIState) 
		{
		case AIState.patrol:
			//action
			if (!agent.pathPending && agent.remainingDistance < 0.5f) {
				NextPoint ();
			}

			//update
			if (alertedToPlayer) //or recieves message 
			{
				curAIState = AIState.pursue;
				//raysphere of size 12 - get all enemies in
				sphereHit = Physics.OverlapSphere(transform.position, 10.0f);
				if (sphereHit.Length != 0) 
				{
					foreach(Collider col in sphereHit)
					{
						if (col.tag == "Enemy") 
						{
							Debug.Log ("Enemy Hit by Sphere Cast!");
							enemyNote = col.GetComponent<IEnemyNotification> ();
							enemyNote.Alert (playerTransf);
						}
					}
				}
			}
			break;

		case AIState.pursue:
			//action
			agent.stoppingDistance = 5.0f;
			agent.autoBraking = true;
			agent.destination = playerTransf.position;

			//update
			if (agent.remainingDistance <= 0.5) {
				curAIState = AIState.attack;
			}
			if (Physics.Raycast(new Ray(transform.position, playerTransf.position - transform.position), out outHit)) 
			{
				if (outHit.collider.gameObject.tag != "Player") 
				{
					lastKnownPos = playerTransf.position;
					playerTransf = null;
					curAIState = AIState.search;
				}
			}
			break;
		case AIState.attack:
			//action
			if ((Time.time - saveTime) > 2f) {
				numShot += 1;
				float yRot = Random.Range (aimRangeBottom, aimRangeTop) * aimFuzz;
				Quaternion aimRot = Quaternion.Euler (0, yRot, 0);
				Vector3 target = aimRot * (playerTransf.position - bulletSpawnT.position);
				Debug.DrawRay (bulletSpawnT.position, target, Color.red);
				if (Physics.Raycast (bulletSpawnT.position, target, out outHit, 7.0f)) {
					if (outHit.collider.gameObject.tag == "Player") {
						//Debug.Log ("Something Hit w/ RayCast:" + outHit.collider.gameObject.name);
					}
				}
				saveTime = Time.time;

				GameObject shotBullet = (GameObject)Instantiate (bulletPrefab, bulletSpawnT.position, Quaternion.identity);
				shotBullet.transform.forward = target;
			}

			//update
			if (agent.remainingDistance > 0.7f) {
				curAIState = AIState.pursue;
			}
			//if player not visible or not in range
			if (!playerInRange) {
				lastKnownPos = playerTransf.position;
				playerTransf = null;
				curAIState = AIState.search;
			}
			else if(Physics.Raycast(new Ray(transform.position, playerTransf.position - transform.position), out outHit))
			{
				if (outHit.collider.gameObject.tag != "Player") {
					lastKnownPos = playerTransf.position;
					playerTransf = null;
					curAIState = AIState.search;
				}
			}
			break;
		case AIState.search:
			//action
			agent.destination = lastKnownPos;
			if (agent.remainingDistance <= 1f) {
				searchTimer = Time.time;
			}

			//update
			if ((Time.time - searchTimer) > 5.5f) {
				alertedToPlayer = false;
				curAIState = AIState.patrol;
				agent.autoBraking = false;
				agent.stoppingDistance = 0.5f;
			}
				break;
		default:
			break;
		}

		/*if (ePT.playerInArea) {
			transform.LookAt (ePT.pTrans);
			//Vector3 target = ePT.pTrans.position + (transform.position.normalized * 5);
			agent.stoppingDistance = 5.0f;
			agent.autoBraking = true;
			agent.destination = ePT.pTrans.position;
			if ((Time.time - saveTime) > 2f) {
				numShot += 1;
				float yRot = Random.Range(aimRangeBottom, aimRangeTop) * aimFuzz;
				Quaternion aimRot = Quaternion.Euler (0, yRot, 0);
				Vector3 target = aimRot * (ePT.pTrans.position - bulletSpawnT.position);
				Debug.DrawRay (bulletSpawnT.position, target, Color.red);
				if (Physics.Raycast (bulletSpawnT.position, target, out outHit, 7.0f)) {
					if (outHit.collider.gameObject.tag == "Player") {
						//Debug.Log ("Something Hit w/ RayCast:" + outHit.collider.gameObject.name);
					}
				}
				saveTime = Time.time;

				GameObject shotBullet = (GameObject)Instantiate (bulletPrefab, bulletSpawnT.position, Quaternion.identity);
				shotBullet.transform.forward = target;
				//Debug.Log ("Number Shot "+ numShot);
				//Debug.Log ("Number Hit "+ numHit);
				//Destroy (shotBullet, 2.0f);
			}
		} else if (!agent.pathPending && agent.remainingDistance < 0.5f) {
			//agent.stoppingDistance = 0.5f;
			NextPoint ();
		} else 
		{
			agent.autoBraking = false;
			agent.stoppingDistance = 0.5f;
		}*/
    }

    void NextPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[destPoint].position;

        destPoint = (destPoint + 1) % patrolPoints.Length;
    }

	public void
	Damage(int damage)
	{
		Debug.Log ("Enemy has been Damaged!");
		Kill();
	}

	public void
	Kill()
	{	
		Debug.Log ("Enemy has been Killed!");
		Destroy (this.gameObject, 0.0f);
	}

	public void
	Alert(Transform aTrans)
	{
		alertedToPlayer = true;
		playerTransf = aTrans;
		curAIState = AIState.pursue;
	}
}
