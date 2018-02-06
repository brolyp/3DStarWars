using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour {

    public GameObject CageDoor;
    public GameObject VictoryZone;    

    public static bool _deadVader = false;

	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {        
        if (_deadVader)
        {
            CageDoor.GetComponent<Renderer>().enabled = false;
            CageDoor.GetComponent<Collider>().enabled = false;
        }
    }
}