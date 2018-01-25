using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPerceptionTrigger : MonoBehaviour {

    public bool playerInArea;
    public Transform pTrans;

	// Use this for initialization
	void Start () {
        playerInArea = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInArea = true;
            pTrans = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player" && playerInArea)
        {
            playerInArea = false;
            pTrans = null;
        }
    }
}
