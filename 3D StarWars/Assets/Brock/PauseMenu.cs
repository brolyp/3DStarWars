using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
	public GameObject lossMenuUI;
	public GameObject winMenuUI;


	public bool lukeDied = false;
	public bool touchLeia = false;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();                
            }

        }
	}

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {

		if (touchLeia == false && lukeDied == false) 
		{
			pauseMenuUI.SetActive (true);
			Time.timeScale = 0f;
			GameIsPaused = true;
		}
    }
    
	public void Restart() 
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);

	}


    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("QUITTING!");
        Application.Quit();
    }



	public void loseGame ()
	{
		if (lukeDied == false) 
		{
			lukeDied = true;
			lossMenuUI.SetActive(true);
			Time.timeScale = 0f;
		}

	}


	public void winGame ()
	{
		if (touchLeia == false) 
		{
			touchLeia = true;
			winMenuUI.SetActive(true);
			Time.timeScale = 0f;
		}

	}

}

