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

    // Metode Update tidak lagi mendeteksi tombol Escape.
    // Sekarang, kamu akan memanggil TogglePause() dari tombol UI.
    void Update()
    {
        // Kode ini sudah tidak diperlukan karena kita akan menggunakan tombol UI.
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     if (Paused)
        //     {
        //         Play();
        //     }
        //     else
        //     {
        //         Stop();
        //     }
        // }
    }

    public void Play()
    {
        PauseMenuCanvas.SetActive(false);
        SettingsMenuCanvas.SetActive(false); // Sembunyikan menu pengaturan saat melanjutkan
        Player.SetActive(true);
        Enemy.SetActive(true);
        UI.SetActive(true); // Tampilkan UI saat melanjutkan game
        Clone.SetActive(true);
        Time.timeScale = 1f;
        Paused = false;
    }

    void Stop()
    {
        PauseMenuCanvas.SetActive(true);
        SettingsMenuCanvas.SetActive(false); // Pastikan menu pengaturan tersembunyi saat menjeda
        Player.SetActive(false);
        Enemy.SetActive(false);
        UI.SetActive(false); // Sembunyikan UI saat game dijeda
        if (Clone != null)
        {
            Clone.SetActive(false); // Pastikan Clone juga dinonaktifkan saat menjeda
        }
        Time.timeScale = 0f;
        Paused = true;
    }

    // Ini adalah metode baru yang akan kamu panggil dari tombol jeda UI.
    public void TogglePause()
    {
        if (Paused)
        {
            Play(); // Jika sedang dijeda, lanjutkan game
        }
        else
        {
            Stop(); // Jika tidak dijeda, jeda game
        }
    }

    public void MainMenuButton()
    {
        // Memastikan game tidak dijeda saat kembali ke menu utama
        Time.timeScale = 1f;
        Paused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ExitButon()
    {
        // game pindah ke halaman SelectingLevel
        Time.timeScale = 1f; // Pastikan game tidak dijeda saat keluar
        Paused = false;
        SceneManager.LoadScene("SelectingLevel");
    }

    public void restart()
    {
        // Memastikan game tidak dijeda saat restart
        Time.timeScale = 1f;
        Paused = false;
        SceneManager.LoadScene("Level1");
        Player.SetActive(false);
        Enemy.SetActive(true);
    }
}