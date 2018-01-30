using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batteryScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void
	OnTriggerEnter(Collider other)
	{
		IHealable isHealable = other.GetComponent<IHealable> ();
		if (isHealable != null)
			isHealable.Heal (100);
	}
}
