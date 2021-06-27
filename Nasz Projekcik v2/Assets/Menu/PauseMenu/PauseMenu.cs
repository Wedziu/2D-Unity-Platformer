using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    GameObject player;

    GameObject kontrolerGry;


    private void Start()
    {
        player = GameObject.Find("Player");
        kontrolerGry = GameObject.Find("KontrolerGry");
    }


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
        Time.timeScale = 1;
        GameIsPaused = false;
        kontrolerGry.SetActive(true);
        player.GetComponent<PlayerInput>().enabled = true;
    }

    void Pause()
    {

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
        kontrolerGry.SetActive(false);
        player.GetComponent<PlayerInput>().enabled = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        kontrolerGry.SetActive(true);
        SceneManager.LoadScene("Menu");      
    }

    public void Reload()
    {
        kontrolerGry.SetActive(true);
        kontrolerGry.GetComponent<KontrolerGry>().ResetStatistic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }



}
