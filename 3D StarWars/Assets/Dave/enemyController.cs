using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyController : MonoBehaviour {

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
        if (ePT.playerInArea)
        {
            transform.LookAt(ePT.pTrans);
            Vector3 target = ePT.pTrans.position - (transform.position.normalized * 5);
            agent.destination = target;
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            NextPoint();
        }
    }

    void NextPoint()
    {
        if (patrolPoints.Length == 0) return;
        Debug.Log("testing");
        agent.destination = patrolPoints[destPoint].position;

        destPoint = (destPoint + 1) % patrolPoints.Length;
    }
}
