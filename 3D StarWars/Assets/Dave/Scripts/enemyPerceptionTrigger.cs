using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPerceptionTrigger : MonoBehaviour {

    public bool playerInArea;
    public Transform pTrans;
	//public GameObject exclamationPointFound;
	//GameObject instantiatedExclaimPF;
	RaycastHit hit;
	public enemyController eControl;

	// Use this for initialization
	void Start () {
        playerInArea = false;
		eControl = GetComponentInParent<enemyController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider other)
	{
		if(other.tag == "Player")
		{
			if(!eControl.playerInRange) eControl.playerInRange = true;
			if (Physics.Raycast (transform.position, other.transform.position - transform.position, out hit,  10.0f )) 
			{
				if (hit.collider.gameObject.tag == "Player") 
				{
					if (Vector3.Dot (transform.forward, other.transform.position - transform.position) > 0) {
						if(!eControl.alertedToPlayer) eControl.alertedToPlayer = true;
						eControl.playerTransf = other.transform;
					}
				}
			}
		}
	}

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
			eControl.playerInRange = false;
			//eControl.playerTransf = null;
        }
    }
}
