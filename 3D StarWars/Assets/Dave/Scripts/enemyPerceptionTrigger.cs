using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPerceptionTrigger : MonoBehaviour {

    public bool playerInArea;
    public Transform pTrans;
	public GameObject exclamationPointFound;
	GameObject instantiatedExclaimPF;
	RaycastHit hit;

	// Use this for initialization
	void Start () {
        playerInArea = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
			if (Physics.Raycast (transform.position, other.transform.position - transform.position, out hit,  7.0f )) 
			{
				Debug.Log ("Something was hit!");
				Debug.Log (hit.collider.gameObject.tag);
				Debug.Log (hit.collider.gameObject.name);
				if (hit.collider.gameObject.tag == "Player") 
				{
					Debug.Log ("The Player was hit!");
					if (Vector3.Dot (transform.forward, other.transform.position - transform.position) > 0) {
						Debug.Log ("The Player is in front of us!");
						playerInArea = true;
						pTrans = other.transform;
						instantiatedExclaimPF = Instantiate (exclamationPointFound, this.transform.position + new Vector3(0,1,0), Quaternion.identity, this.transform );
						Destroy (instantiatedExclaimPF, 1.0f);
					}
				}
			}
            //playerInArea = true;
            //pTrans = other.transform;
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
