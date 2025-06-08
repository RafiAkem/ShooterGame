using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    public Image buttonImage;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            UpdateButtonImage();
        }
        else
        {
            Debug.LogError("SoundManager tidak ditemukan di scene!");
        }
    }

    public void ToggleMute()
    {
        if (SoundManager.Instance != null)
        {
            bool newMuteState = !SoundManager.Instance.IsMuted();
            SoundManager.Instance.SetMute(newMuteState);
            UpdateButtonImage();
            Debug.Log("Sound muted: " + newMuteState);
        }
        else
        {
            Debug.LogError("SoundManager.Instance masih null saat ToggleMute dipanggil.");
        }
    }

    private void UpdateButtonImage()
    {
        if (buttonImage != null && SoundManager.Instance != null)
        {
            bool isMuted = SoundManager.Instance.IsMuted();
            buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }
    }

    // Opsional: jika ingin update gambar setiap kali toggle muncul lagi
    private void OnEnable()
    {
        UpdateButtonImage();
    }
}
