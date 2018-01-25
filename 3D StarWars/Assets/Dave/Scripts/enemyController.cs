using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyController : MonoBehaviour, IDamageable, IKillable {

    public Transform[] patrolPoints;
    public enemyPerceptionTrigger ePT;
    public int destPoint;
    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        ePT = GetComponentInChildren<enemyPerceptionTrigger>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        destPoint = 0;
        NextPoint();
	}
	
	// Update is called once per frame
	void Update () {
		if (ePT.playerInArea) {
			transform.LookAt (ePT.pTrans);
			//Vector3 target = ePT.pTrans.position + (transform.position.normalized * 5);
			agent.stoppingDistance = 5.0f;
			agent.autoBraking = true;
			agent.destination = ePT.pTrans.position;
		} else if (!agent.pathPending && agent.remainingDistance < 0.5f) {
			//agent.stoppingDistance = 0.5f;
			NextPoint ();
		} else 
		{
			agent.autoBraking = false;
			agent.stoppingDistance = 0.5f;
		}
    }

    void NextPoint()
    {
        if (patrolPoints.Length == 0) return;
        Debug.Log("testing");
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
}
