using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 50f;
    public float moveRange;
    private float startY;
    private bool movingUp = true;

    public int maxHealth = 3;
    private int currentHealth;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public GameObject enemyBulletPrefab;
    public Transform firePoint; // assign an empty child transform where bullets come from
    public float fireRate = 6f; // seconds between shots
    private float nextFireTime = 0f;

    public GameObject explosionPrefab;

    public AudioClip hitSound;
    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startY = transform.position.y;
        currentHealth = maxHealth;

        // Get the SpriteRenderer and save original color
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        // Calculate vertical range based on camera height and position
        float camHeight = Camera.main.orthographicSize;
        float minY = Camera.main.transform.position.y - camHeight + 2f;
        float maxY = Camera.main.transform.position.y + camHeight - 2f;

        // Clamp range so enemy never leaves screen
        float topLimit = maxY - transform.position.y;
        float bottomLimit = transform.position.y - minY;
        moveRange = Mathf.Min(topLimit, bottomLimit);

        nextFireTime = Time.time + Random.Range(0f, fireRate);
    }

    void Update()
    {
        float direction = movingUp ? 1 : -1;
        transform.Translate(Vector3.up * direction * speed * Time.deltaTime);

        if (transform.position.y > startY + moveRange)
            movingUp = false;
        else if (transform.position.y < startY - moveRange)
            movingUp = true;

        if (transform.position.x < -Camera.main.orthographicSize * Camera.main.aspect - 2f)
            Destroy(gameObject);

                    // Shooting logic
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

void Shoot()
{

    Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject);   // destroy the bullet
            TakeDamage(1);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Start flashing coroutine
        StartCoroutine(FlashOnHit());

        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (currentHealth <= 0) {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private IEnumerator FlashOnHit()
    {
        spriteRenderer.color = Color.red; // flash color
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor; // back to original
    }
}
