using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int maxHealth = 300;
    private int currentHealth;

    public float moveSpeed = 3f;       // Leftward speed
    public float verticalAmplitude = 1f;  // How far it moves up/down
    public float verticalFrequency = 1f;  // Speed of vertical oscillation

    private Vector3 startPosition;
    public GameObject bulletPrefab;
    public Transform[] firePoints;
    public float attackInterval = 2f;
    public float bulletSpeed = 5f;
    private int currentPhase = 1;


    void Start()
    {
        currentHealth = maxHealth;
        startPosition = transform.position;

        StartCoroutine(FireLoop());
    }

    void Update()
    {
        MoveLeftWithVerticalOscillation();
    }

void MoveLeftWithVerticalOscillation()
{
    float stopX = Camera.main.transform.position.x + 2f; // Stop 4 units from left edge
    float currentX = transform.position.x;

    // Move left only if not yet reached stopX
    if (currentX > stopX)
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    // Always oscillate vertically
    float newY = startPosition.y + Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
    transform.position = new Vector3(transform.position.x, newY, transform.position.z);
}


    public void TakeDamage(int damage)
    {
    currentHealth -= damage;
    Debug.Log("Boss took damage: " + damage + ", current HP: " + currentHealth);

    UpdatePhase();

    if (currentHealth <= 0)
    {
        Die();
    }
    }

    void UpdatePhase()
{
    if (currentHealth <= 200 && currentPhase == 1)
    {
        currentPhase = 2;
        Debug.Log("Phase 2 started!");
    }
    else if (currentHealth <= 100 && currentPhase == 2)
    {
        currentPhase = 3;
        Debug.Log("Phase 3 started!");
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
            TakeDamage(1);
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
            FireSpread();
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

void FireSpread()
{
    foreach (Transform point in firePoints)
    {
        FireAtAngle(point.position, -10f);
        FireAtAngle(point.position, 0f);
        FireAtAngle(point.position, 10f);
    }
}

void FireAtAngle(Vector3 pos, float angle)
{
    GameObject bullet = Instantiate(bulletPrefab, pos, Quaternion.Euler(0, 0, angle));
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
