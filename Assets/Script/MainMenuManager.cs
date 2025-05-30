using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("SelectLevel");
    }


    public void SettingsButton()
    {
        SceneManager.LoadScene("Setting");
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void backbutton() {
        SceneManager.LoadScene("MainMenu");
    }

    public void CreditsButton() {
        SceneManager.LoadScene("Credit");
    }
}
