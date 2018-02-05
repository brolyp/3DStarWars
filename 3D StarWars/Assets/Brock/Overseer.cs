using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Overseer : MonoBehaviour {

	public bool lukeDied = false;
	public bool vaderDied = false;

	public void loseGame ()
	{
		if (lukeDied == false) 
		{
			lukeDied = true;
			SceneManager.LoadScene("MainMenu");
		}

	}


	public void winGame ()
	{
		if (vaderDied == false) 
		{
			vaderDied = true;
			SceneManager.LoadScene("Game");
		}

	}



}