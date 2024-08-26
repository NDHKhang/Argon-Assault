using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    // For display menu
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;
    // For enable/disable player control
    [SerializeField] GameObject playerShip;

    public static LoadManager instance;

    void Start()
    { 
        instance = this;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Pause()
    {
            Time.timeScale = 0;
            playerShip.GetComponent<PlayerControls>().enabled = false;
            pauseMenu.SetActive(true);
            AudioListener.pause = true;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        playerShip.GetComponent<PlayerControls>().enabled = true;
        pauseMenu.SetActive(false);
        AudioListener.pause = false;
    }

    public void RestartLevel()
    {
        Resume();
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        playerShip.GetComponent<PlayerControls>().enabled = false;
        gameOverMenu.SetActive(true);
        AudioListener.pause = true;
    }
}
