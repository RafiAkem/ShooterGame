using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FixedJoystick joystick;
    public float moveSpeed = 100f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 20.0f; // Delay between shots
    private float nextFireTime;

    public bool canShoot = true;
    public bool canMove = true;  // New flag to enable/disable movement

    void Update()
    {
        if (!canMove) return;  // Skip movement and shooting when disabled

        // Movement
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;

        Vector3 move = new Vector3(moveX, moveY, 0f);
        transform.position += move * moveSpeed * Time.deltaTime;

        // Clamp to screen
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.05f, 0.95f);
        pos.y = Mathf.Clamp(pos.y, 0.05f, 0.95f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        // Shooting with cooldown
        if (canShoot && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject);

            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeEnemyBulletHit();
            }
            else
            {
                Debug.LogWarning("PlayerHealth component not found on Player!");
            }
        }
    }
}
