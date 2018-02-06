using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
	public GameObject lossMenuUI;
	public GameObject winMenuUI;
    public TextMeshProUGUI pauseScore;
    public TextMeshProUGUI winScore;
    public TextMeshProUGUI loseScore;

    public bool lukeDied = false;
	public bool touchLeia = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }   

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
		if (touchLeia == false && lukeDied == false) 
		{
			pauseScore.text = FindObjectOfType<KillCountControl> ().getKillCount().ToString();
            winScore.text = FindObjectOfType<KillCountControl>().getKillCount().ToString();
            loseScore.text = FindObjectOfType<KillCountControl>().getKillCount().ToString();
            pauseMenuUI.SetActive (true);
			Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
			Time.timeScale = 0f;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			GameIsPaused = true;
			lukeDied = true;
			lossMenuUI.SetActive(true);
			Time.timeScale = 0f;
		}

	}


	public void winGame ()
	{
		if (touchLeia == false) 
		{
			Time.timeScale = 0f;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			GameIsPaused = true;
			touchLeia = true;
			winMenuUI.SetActive(true);
		}

	}

}

