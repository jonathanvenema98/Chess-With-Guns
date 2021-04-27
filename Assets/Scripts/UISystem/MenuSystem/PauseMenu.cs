using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public GameObject gameUI;

    private State previousState;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        gameUI.SetActive(true);
        //Time.timeScale = 1f;
        GameIsPaused = false;
        StateMachine.Instance.SetState(previousState);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        gameUI.SetActive(false);
        //Time.timeScale = 0f;
        GameIsPaused = true;
        previousState = StateMachine.Instance.State;
        StateMachine.Instance.SetState(new PausedState());
    }

    public void LoadMenu()
    {
        Debug.Log("Loading menu...");
        Resume();
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
