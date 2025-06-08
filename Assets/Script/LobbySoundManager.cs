using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySoundManager : MonoBehaviour
{
    public AudioClip lobbyMusic;
    private AudioSource audioSource;

    void Awake()
    {
        // Ensure only one lobby music exists and persists between scenes
        GameObject[] musicObjs = GameObject.FindGameObjectsWithTag("LobbyMusic");
        if (musicObjs.Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = lobbyMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}
