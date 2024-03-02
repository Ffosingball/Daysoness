using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    public bool PauseGame=false; 
    public GameObject PauSee, textus;
    public statistica stats;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(PauseGame)
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
        mousePicture.change_coursor_target();
        PauSee.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;
        textus.SetActive(true);
    }

    public void Pause()
    {
        mousePicture.change_coursor_menu();
        PauSee.SetActive(true);
        Time.timeScale = 0f;
        PauseGame = true;
        textus.SetActive(false);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu");
    }

    public void Statisticus()
    {
        PauSee.SetActive(false);
        stats.showStatistic();
    }

    public void Back()
    {
        PauSee.SetActive(true);
        stats.hideStatistic();
    }
}
