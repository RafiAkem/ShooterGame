using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int maxHealth = 150;
    private int currentHealth;

    public float moveSpeed = 3f;       // Leftward speed
    public float verticalAmplitude = 2f;  // How far it moves up/down
    public float verticalFrequency = 2f;  // Speed of vertical oscillation

    private Vector3 startPosition;
    public GameObject bulletPrefab;
    public GameObject phase2BulletPrefab;

    public Transform[] firePoints;
    public Transform[] phase2FirePoints;
    public float attackInterval = 2f;
    public float bulletSpeed = 5f;
    private int currentPhase = 1;
    private float stopX;
    private float bossHalfWidth;

    public GameObject explosionEffect;
    public Transform[] explosionPoints;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        startPosition = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        // Calculate boss half width based on SpriteRenderer bounds
        if (spriteRenderer != null)
        {
            bossHalfWidth = spriteRenderer.bounds.size.x / 2f;
        }

        float screenRightX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        stopX = screenRightX - bossHalfWidth - 0.5f; // 0.5f is extra padding

        StartCoroutine(FireLoop());
    }

    void Update()
    {
        MoveLeftWithVerticalOscillation();
    }

    void MoveLeftWithVerticalOscillation()
    {
        float currentX = transform.position.x;

        if (currentX > stopX)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }

        float newY = startPosition.y + Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Boss took damage: " + damage + ", current HP: " + currentHealth);

        if (spriteRenderer != null)
            StartCoroutine(FlashOnHit());

        UpdatePhase();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashOnHit()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    IEnumerator TriggerExplosionsDelayed()
    {
        if (explosionEffect == null || explosionPoints == null) yield break;

        foreach (Transform point in explosionPoints)
        {
            Instantiate(explosionEffect, point.position, Quaternion.identity);
            yield return new WaitForSeconds(0.3f); // Delay between explosions
        }
    }

    void UpdatePhase()
    {
        if (currentHealth <= 100 && currentPhase == 1)
        {
            currentPhase = 2;
            Debug.Log("Phase 2 started!");
            StartCoroutine(TriggerExplosionsDelayed());
        }
        else if (currentHealth <= 50 && currentPhase == 2)
        {
            currentPhase = 3;
            Debug.Log("Phase 3 started!");
            StartCoroutine(TriggerExplosionsDelayed());
        }
    }

    void Die()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject);
            TakeDamage(5);
        }
    }

    IEnumerator FireLoop()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(attackInterval);
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoints == null) return;

        switch (currentPhase)
        {
            case 1:
                FireStraight();
                break;
            case 2:
                FireSpreadPhase2();
                break;
            case 3:
                FireWave();
                break;
        }
    }

    void FireStraight()
    {
        foreach (Transform point in firePoints)
        {
            GameObject bullet = Instantiate(bulletPrefab, point.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = Vector2.left * bulletSpeed;
        }
    }

    IEnumerator FireCircularBurst(Transform firePoint)
{
    int bulletCount = 20; 
    float delayBetweenBullets = 0.02f;
    float angleStep = 360f / bulletCount;

    for (int i = 0; i < bulletCount; i++)
    {
        float angle = i * angleStep;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector2 direction = rotation * Vector2.right;

        GameObject bullet = Instantiate(phase2BulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction.normalized * bulletSpeed;
        }

        yield return new WaitForSeconds(delayBetweenBullets);
    }
}


void FireSpreadPhase2()
{
    if (phase2FirePoints == null || phase2FirePoints.Length == 0 || phase2BulletPrefab == null) return;

    foreach (Transform point in phase2FirePoints)
    {
        StartCoroutine(FireCircularBurst(point));
    }
}

void FireAtAngle(Vector3 pos, float angle, GameObject customBullet = null)
{
    GameObject prefabToUse = customBullet != null ? customBullet : bulletPrefab;

    GameObject bullet = Instantiate(prefabToUse, pos, Quaternion.Euler(0, 0, angle));
    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.left;
        rb.velocity = dir.normalized * bulletSpeed;
    }
}

    void FireWave()
    {
        float time = Time.time * 10f;
        foreach (Transform point in firePoints)
        {
            float waveOffset = Mathf.Sin(time) * 20f; // -20 to +20 degrees
            FireAtAngle(point.position, waveOffset);
        }
    }
}
