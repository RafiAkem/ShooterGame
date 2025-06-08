using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public static bool Paused = false;
    public GameObject PauseMenuCanvas;
    public GameObject SettingsMenuCanvas;
    public GameObject Player;
    public GameObject Enemy;
    public GameObject UI;
    public GameObject Clone;

    void Start()
    {
        Time.timeScale = 1f;
        PauseMenuCanvas.SetActive(false);
        SettingsMenuCanvas.SetActive(false);
    }

    void Update()
    {
        // Pause is now toggled via UI button, not by Escape key
    }

    public void Play()
    {
        PauseMenuCanvas.SetActive(false);
        SettingsMenuCanvas.SetActive(false);
        Player.SetActive(true);
        Enemy.SetActive(true);
        UI.SetActive(true);
        if (Clone != null) Clone.SetActive(true);

        SetBulletVisibility(true); // Show bullets again

        Time.timeScale = 1f;
        Paused = false;
    }

    void Stop()
    {
        PauseMenuCanvas.SetActive(true);
        SettingsMenuCanvas.SetActive(false);
        Player.SetActive(false);
        Enemy.SetActive(false);
        UI.SetActive(false);
        if (Clone != null) Clone.SetActive(false);

        SetBulletVisibility(false); // Hide bullets visually

        Time.timeScale = 0f;
        Paused = true;
    }

    public void TogglePause()
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

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        Paused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ExitButon()
    {
        Time.timeScale = 1f;
        Paused = false;
        SceneManager.LoadScene("SelectingLevel");
    }

    public void restart()
    {
        Time.timeScale = 1f;
        Paused = false;
        SceneManager.LoadScene("Level1");
        Player.SetActive(false);
        Enemy.SetActive(true);
    }

    // Helper to show/hide bullets visually (not deactivate them)
    void SetBulletVisibility(bool visible)
    {
        GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");
        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

        foreach (GameObject bullet in playerBullets)
        {
            SpriteRenderer sr = bullet.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = visible;
        }

        foreach (GameObject bullet in enemyBullets)
        {
            SpriteRenderer sr = bullet.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = visible;
        }
    }
}
