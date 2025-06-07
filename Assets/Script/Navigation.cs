using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    
    public void GoToSelectLevel()
    {
        SceneManager.LoadScene("SelectingLevel");
    }

    public void ExitGame()
    {
        // Keluar dari aplikasi
        Application.Quit();

        // Jika dijalankan di editor, gunakan ini untuk menghentikan permainan
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void GoTosettings()
    {
        SceneManager.LoadScene("Setting");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToCredits() 
    {
        SceneManager.LoadScene("Credit");
    }

    public void GoToLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
}
