using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    public Image buttonImage;
    public Sprite soundOnSprite; 
    public Sprite soundOffSprite;

    private bool isMuted = false;

    private void Start()
    {
        UpdateButtonImage();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        SoundManager.Instance.SetMute(isMuted);
        UpdateButtonImage();
        Debug.Log("Sound muted: " + isMuted);
    }

    private void UpdateButtonImage()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }
    }
}
