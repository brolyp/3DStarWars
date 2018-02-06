using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class enemyController : MonoBehaviour, IDamageable, IKillable, IEnemyNotification {

	public enum AIState {patrol, pursue, attack, search};
	public AIState curAIState;

	NavMeshAgent agent;

	public Transform[] patrolPoints;
	public Transform bulletSpawnT;
	public Transform rifle;
	public Transform playerTransf;

	public GameObject bulletPrefab;
	public GameObject exclamationPointFound;
	GameObject instantiatedExclaimPF;

	public Vector3 lastKnownPos;
	public Vector3 target;

	public float aimFuzz;
	public float aimRangeBottom;
	public float aimRangeTop;
	public float shootTimer;
	public float ignoreDist;
	public float searchTime;

	public int destPoint;

	public bool alertedToPlayer;
	public bool playerInRange;
	bool startedTimer;

	public float distance;

	RaycastHit outHit;
	Collider[] sphereHit;

	IEnemyNotification enemyNote;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
		//aimFuzz = 15.0f;
		aimRangeTop = 10.0f;
		aimRangeBottom = -10f;
		shootTimer = 0.0f;
		searchTime = 0.0f;
		alertedToPlayer = false;

		destPoint = 0;
		NextPoint();
	}
	
	// Update is called once per frame
	void Update () {
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
			agent.stoppingDistance = 10.0f;
			agent.autoBraking = true;
			agent.destination = playerTransf.position;

			distance = Vector3.Distance (this.transform.position, playerTransf.position);

			//update
			if (agent.remainingDistance <= 10) {
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

			distance = Vector3.Distance (this.transform.position, playerTransf.position);
			transform.LookAt (playerTransf.position);
			if ((Time.time - shootTimer) > 2f) {
				aimFuzz = Vector3.Distance (transform.position, playerTransf.position);
				Mathf.Clamp (aimFuzz, 0f, 10f);
				float yRot = Random.Range (aimRangeBottom, aimRangeTop);// * aimFuzz;
				Quaternion aimRot = Quaternion.Euler (0, yRot, 0);
				Vector3 target = aimRot * (playerTransf.position - bulletSpawnT.position);
				shootTimer = Time.time;
				GameObject shotBullet = (GameObject)Instantiate (bulletPrefab, bulletSpawnT.position, Quaternion.identity);
				shotBullet.transform.forward = target;
			}

			Vector3 revToPlayer = transform.position - playerTransf.position;
			if (Vector3.Distance (playerTransf.position, transform.position) < 4.0f && !agent.pathPending) {
				target = transform.position + (revToPlayer.normalized * 5f);
				if (agent.SetDestination (target))
					Debug.Log ("Setting Destination has gone right?");
				agent.stoppingDistance = 0.5f;
			}

			//update
			if (agent.remainingDistance > 11.0f) {
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
			if (!agent.hasPath && searchTime == 0) {
				searchTime = Time.time;
			}

			//update
			if (searchTime != 0 && (Time.time - searchTime) > 5.5f) {
				searchTime = 0f;
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
