using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    public Image buttonImage;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private const string MutePrefKey = "SoundMuted";

    private void Start()
    {
        // Load saved mute state and apply it
        bool isMuted = PlayerPrefs.GetInt(MutePrefKey, 0) == 1;
        AudioListener.pause = isMuted;

        UpdateButtonImage();
    }

    public void ToggleMute()
    {
        bool isMuted = !AudioListener.pause;

        // Apply new mute state
        AudioListener.pause = isMuted;

        // Save to PlayerPrefs
        PlayerPrefs.SetInt(MutePrefKey, isMuted ? 1 : 0);
        PlayerPrefs.Save();

        UpdateButtonImage();

        Debug.Log("Sound muted: " + isMuted);
    }

    private void UpdateButtonImage()
    {
        if (buttonImage != null)
        {
            bool isMuted = AudioListener.pause;
            buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }
    }

    private void OnEnable()
    {
        UpdateButtonImage();
    }
}
