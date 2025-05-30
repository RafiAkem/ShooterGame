using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public static bool Paused = false;
    public GameObject PauseMenuCanvas;
    public GameObject SettingsMenuCanvas; 

    void Start()
    {
        Time.timeScale = 1f;
        PauseMenuCanvas.SetActive(false); 
        SettingsMenuCanvas.SetActive(false); 
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }
    }

    public void Play()
    {
        PauseMenuCanvas.SetActive(false);
        SettingsMenuCanvas.SetActive(false); // Hide settings menu when resuming
        Time.timeScale = 1f;
        Paused = false;
    }

    void Stop()
    {
        PauseMenuCanvas.SetActive(true);
        SettingsMenuCanvas.SetActive(false); // Ensure settings menu is hidden when pausing
        Time.timeScale = 0f;
        Paused = true;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void BackButton()
    {
        SceneManager.LoadScene("SelectLevel");
    }
}
