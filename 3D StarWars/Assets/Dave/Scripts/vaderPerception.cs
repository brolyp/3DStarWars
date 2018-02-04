using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vaderPerception : MonoBehaviour {

	public bool playerInArea;
	public Transform pTrans;
	RaycastHit hit;
	public vaderControl vControl;

	// Use this for initialization
	void Start () {
		playerInArea = false;
		vControl = GetComponentInParent<vaderControl> ();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			vControl.playerInRange = true;
			if (Physics.Raycast (transform.position, other.transform.position - transform.position, out hit,  10.0f )) 
			{
				if (hit.collider.gameObject.tag == "Player") 
				{
					if (Vector3.Dot (transform.forward, other.transform.position - transform.position) > 0) {
						//instantiatedExclaimPF = Instantiate (exclamationPointFound, this.transform.position + new Vector3(0,1,0), Quaternion.identity, this.transform );
						//Destroy (instantiatedExclaimPF,0.5f);
					}
				}
			}
		}
		if (other.name == "Bullet(Clone)") {
			Debug.Log ("Bullet Detected!");
			vControl.firedAt = true;
			vControl.bulletTransf = other.transform;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.tag == "Player")
		{
			if(!vControl.playerInRange) vControl.playerInRange = true;
			if (Physics.Raycast (transform.position, other.transform.position - transform.position, out hit,  10.0f )) 
			{
				if (hit.collider.gameObject.tag == "Player") 
				{
					if (Vector3.Dot (transform.forward, other.transform.position - transform.position) > 0) {
						if(!vControl.alertedToPlayer) vControl.alertedToPlayer = true;
						vControl.playerTransf = other.transform;
					}
				}
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			vControl.playerInRange = false;
			//eControl.playerTransf = null;
		}
		if (other.name == "Bullet") {
			vControl.firedAt = false;
			vControl.bulletTransf = null;
		}
	}
}
