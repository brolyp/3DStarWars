using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {

    public float respawnTimer = 0;
    private float timeLeft = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.deltaTime != 0)
        {
            if (GetComponent<Renderer>().enabled == false)
            {
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0)
                {
                    GetComponent<Renderer>().enabled = true;
                    GetComponent<Collider>().enabled = true;
                    timeLeft = respawnTimer;
                }
            }
        }
    }
}
