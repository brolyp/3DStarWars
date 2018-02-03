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
	public float ignoreDist;
	public GameObject exclamationPointFound;
	GameObject instantiatedExclaimPF;
	public float dist;
	public float searchTime;
	public bool repositioning;
	public Vector3 target;

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
		searchTimer = 0;
		alertedToPlayer = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerTransf != null)
			dist = Vector3.Distance (playerTransf.position, transform.position);
		switch (curAIState) 
		{
		case AIState.patrol:
			//action
			agent.stoppingDistance = 0.5f;
			if (!agent.pathPending && agent.remainingDistance < 0.5f) {
				NextPoint ();
			}

			//update
			if (alertedToPlayer) //or recieves message 
			{
				curAIState = AIState.pursue;
				instantiatedExclaimPF = Instantiate (exclamationPointFound, this.transform.position + new Vector3(0,1,0), Quaternion.identity, this.transform );
				Destroy (instantiatedExclaimPF,0.5f);
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
			if (agent.remainingDistance <= 5.1) {
				curAIState = AIState.attack;
			}
			if (Physics.Raycast(new Ray(transform.position, playerTransf.position - transform.position), out outHit)) 
			{
				if (outHit.collider.gameObject.tag != "Player") 
				{
					lastKnownPos = playerTransf.position;
					agent.destination = lastKnownPos;
					playerTransf = null;
					curAIState = AIState.search;
					alertedToPlayer = false;
					agent.stoppingDistance = 0.5f;
				}
			}
			break;
		case AIState.attack:
			//action
			transform.LookAt (playerTransf.position);
			if ((Time.time - saveTime) > 2f) {
				numShot += 1;
				float yRot = Random.Range (aimRangeBottom, aimRangeTop) * aimFuzz;
				Quaternion aimRot = Quaternion.Euler (0, yRot, 0);
				Vector3 target = aimRot * (playerTransf.position - bulletSpawnT.position);
				saveTime = Time.time;

				GameObject shotBullet = (GameObject)Instantiate (bulletPrefab, bulletSpawnT.position, Quaternion.identity);
				shotBullet.transform.forward = target;
			}

			//Vector3 toPlayer = playerTransf.position - transform.position;
			Vector3 revToPlayer = transform.position - playerTransf.position;
			//Debug.DrawRay (transform.position, toPlayer, Color.red);
			if (Vector3.Distance (playerTransf.position, transform.position) < 4.0f && !agent.pathPending) {
				target = transform.position + (revToPlayer.normalized * 4.5f);
				instantiatedExclaimPF = Instantiate (exclamationPointFound, target, Quaternion.identity, this.transform);
				Destroy (instantiatedExclaimPF, 0.2f);
				if (agent.SetDestination (target))
					Debug.Log ("Setting Destination has gone right?");
				agent.stoppingDistance = 0.5f;
			}

			//update
			if (agent.remainingDistance > 6.0f) {
				curAIState = AIState.pursue;
			}

			if(Physics.Raycast(new Ray(transform.position, playerTransf.position - transform.position), out outHit))
			{
				if (outHit.collider.gameObject.tag != "Player") {
					lastKnownPos = playerTransf.position;
					agent.destination = lastKnownPos;
					playerTransf = null;
					curAIState = AIState.search;
					alertedToPlayer = false;
					agent.stoppingDistance = 0.5f;
				}
			}
			break;

		case AIState.search:
			//action
			if (alertedToPlayer) {
				curAIState = AIState.pursue;
			}
			if (!agent.hasPath && searchTimer == 0) {
				searchTimer = Time.time;
			}

			//update
			if (searchTimer != 0 && (Time.time - searchTimer) > 5.5f) {
				searchTimer = 0f;
				alertedToPlayer = false;
				curAIState = AIState.patrol;
				agent.autoBraking = false;
				agent.stoppingDistance = 0.5f;
			}
				break;
		default:
			break;
		}
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
		float dist = Vector3.Distance (aTrans.position, transform.position);
		if (dist <= ignoreDist) 
		{
			playerTransf = aTrans;
			curAIState = AIState.pursue;
			alertedToPlayer = true;
		}
	}
}
