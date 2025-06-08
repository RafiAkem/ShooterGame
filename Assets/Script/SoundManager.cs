using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private AudioSource audioSource;

    public AudioClip loseSoundClip;

    private bool isMuted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayLoseSound(float fadeDuration = 2f)
    {
        if (loseSoundClip != null)
        {
            StartCoroutine(PlayLoseSoundFadeIn(fadeDuration));
        }
    }

    private IEnumerator PlayLoseSoundFadeIn(float fadeDuration)
    {
        audioSource.clip = loseSoundClip;
        audioSource.volume = 0f;
        audioSource.Play();

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }

        audioSource.volume = 1f;
    }

    public void SetMute(bool mute)
    {
        isMuted = mute;

        audioSource.mute = mute;

        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in allAudioSources)
        {
            if (a != audioSource)
                a.mute = mute;
        }
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    // This toggles mute state; connect this to your Toggle's OnValueChanged(bool)
    public void ToggleMute(bool mute)
    {
        SetMute(mute);
    }
}
