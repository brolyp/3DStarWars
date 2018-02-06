using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCollider : MonoBehaviour {

    public PauseMenu PauseMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && Victory._deadVader)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        PauseMenu.winGame();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
