using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExit : MonoBehaviour
{
    public float exitSpeed = 5f;
    private bool isExiting = false;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void StartExit()
    {
        isExiting = true;

        if (playerController != null)
        {
            playerController.canMove = false;  // Disable movement on exit
            playerController.canShoot = false;
        }
    }

    void Update()
    {
        if (isExiting)
        {
            transform.position += Vector3.right * exitSpeed * Time.deltaTime;

            if (transform.position.x > Camera.main.ViewportToWorldPoint(new Vector3(1.2f, 0, 0)).x)
                isExiting = false;
        }
    }
}

