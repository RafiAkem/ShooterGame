using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossController : MonoBehaviour
{
    public int maxHealth = 150;
    private int currentHealth;

    public float moveSpeed = 3f;
    public float verticalAmplitude = 2f;
    public float verticalFrequency = 2f;

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

    public GameObject enemyPrefab;
    public GameObject homingMissilePrefab;
    public Transform[] missileSpawnPoints;

    private bool isInvincible = false;
    private bool isRetreating = false;
    public float retreatSpeed = 6f;
    private bool isPhase3FinalAttack = false;
    public TextMeshProUGUI levelCompleteText;

    private List<GameObject> phase3Enemies = new List<GameObject>();

    void Start()
    {
        currentHealth = maxHealth;
        startPosition = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        if (spriteRenderer != null)
            bossHalfWidth = spriteRenderer.bounds.size.x / 2f;

        float screenRightX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        stopX = screenRightX - bossHalfWidth - 0.5f;

        StartCoroutine(FireLoop());
    }

    void Update()
    {
        if (!isRetreating)
            MoveLeftWithVerticalOscillation();
    }

    void MoveLeftWithVerticalOscillation()
    {
        float currentX = transform.position.x;

        if (currentX > stopX)
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        float newY = startPosition.y + Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        Debug.Log("Boss took damage: " + damage + ", current HP: " + currentHealth);

        if (spriteRenderer != null)
            StartCoroutine(FlashOnHit());

        UpdatePhase();

        if (currentHealth <= 0)
            Die();
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
            yield return new WaitForSeconds(0.3f);
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
            isInvincible = true;
            StartCoroutine(TriggerExplosionsDelayed());
            StartCoroutine(EnterPhase3());
        }
    }

void Die()
{
    Debug.Log("Boss defeated!");
    StopAllCoroutines();
    isInvincible = true;

    StartCoroutine(BossDeathSequence());
}


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject);
            TakeDamage(2);
        }
    }

IEnumerator FireLoop()
{
    while (true)
    {
        if (currentPhase != 3)
        {
            Shoot();
        }
        else if (isPhase3FinalAttack)
        {
            // Fire everything!
            FireStraight();
            FireSpreadPhase2();
            FireWave();
        }

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
                rb.velocity = direction.normalized * bulletSpeed;

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
            float waveOffset = Mathf.Sin(time) * 20f;
            FireAtAngle(point.position, waveOffset);
        }
    }

IEnumerator EnterPhase3()
{
    isRetreating = true;

    float targetX = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 0, 0)).x;

    while (transform.position.x < targetX)
    {
        transform.position += Vector3.right * retreatSpeed * Time.deltaTime;
        yield return null;
    }

    yield return new WaitForSeconds(1f);

    SpawnPhase3Enemies();
    StartCoroutine(FireMissileRoutine());
}


IEnumerator FireMissileRoutine()
{
    Debug.Log("Missile coroutine started");

    while (currentPhase == 3)
    {
        yield return new WaitForSeconds(2f);

        if (homingMissilePrefab != null && missileSpawnPoints != null)
        {
            foreach (Transform point in missileSpawnPoints)
            {
                Instantiate(homingMissilePrefab, point.position, Quaternion.identity);
                Debug.Log("Missile spawned at: " + point.position);
            }
        }

        yield return new WaitForSeconds(3f);
    }
}

    IEnumerator LoopExplosionsWhileRetreating()
{
    while (isRetreating)
    {
        if (explosionEffect != null && explosionPoints != null)
        {
            foreach (Transform point in explosionPoints)
            {
                Instantiate(explosionEffect, point.position, Quaternion.identity);
            }
        }

        yield return new WaitForSeconds(0.4f); // Adjust timing for pacing
    }
}

    void SpawnPhase3Enemies()
    {
        phase3Enemies.Clear();

        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        SpriteRenderer sr = enemyPrefab.GetComponent<SpriteRenderer>();
        float enemyWidth = sr != null ? sr.bounds.size.x : 1f;
        float enemyHeight = sr != null ? sr.bounds.size.y : 1f;

        float spacingX = enemyWidth + 1f;
        float spacingY = enemyHeight + 0.5f;

        float pyramidWidth = 3 * spacingX;
        float leftEdge = Camera.main.transform.position.x + camWidth - pyramidWidth + (spacingX / 2f);
        float centerY = Camera.main.transform.position.y;

        for (int col = 0; col < 3; col++)
        {
            int enemiesInColumn = col + 1;
            float columnHeight = (enemiesInColumn - 1) * spacingY;
            float startY = centerY - columnHeight / 2f;
            float x = leftEdge + col * spacingX;

            for (int row = 0; row < enemiesInColumn; row++)
            {
                float y = startY + row * spacingY;
                Vector3 spawnPos = new Vector3(x, y, 0f);

                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                phase3Enemies.Add(enemy);
            }
        }

        StartCoroutine(WaitForEnemiesThenReturn());
    }

IEnumerator WaitForEnemiesThenReturn()
{
    // Wait until all phase3Enemies are null
    while (phase3Enemies.Exists(e => e != null))
    {
        yield return null;
    }

    StartCoroutine(BossReturn());
}

    IEnumerator BossReturn()
{
    float returnSpeed = retreatSpeed;

    // Move boss back to original stopX position
    while (transform.position.x > stopX)
    {
        transform.position += Vector3.left * returnSpeed * Time.deltaTime;
        yield return null;
    }

    isRetreating = false;
    isInvincible = false;

    isPhase3FinalAttack = true;

    Debug.Log("Boss returned from retreat.");
}

IEnumerator BossDeathSequence()
{
    float explosionDuration = 6f;
    float timer = 0f;

    // 1. Continuous random explosions for 3 seconds
    while (timer < explosionDuration)
    {
        if (explosionEffect != null)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
            Instantiate(explosionEffect, transform.position + randomOffset, Quaternion.identity);
        }

        timer += 0.8f;
        yield return new WaitForSeconds(0.2f);
    }

    // 2. Final big explosion
    Instantiate(explosionEffect, transform.position, Quaternion.identity); // You could swap to a 'big' prefab
    yield return new WaitForSeconds(0.5f);

    // 3. Boss disappears
    gameObject.SetActive(false);

    // 4. Trigger player ship to fly away
    GameObject player = GameObject.FindGameObjectWithTag("PlayerShip");
    if (player != null)
    {
        PlayerExit playerExit = player.GetComponent<PlayerExit>();
        if (playerExit != null)
        {
            playerExit.StartExit();
        }
    }

    // 5. Show "LEVEL COMPLETE" after a delay
    yield return new WaitForSeconds(2f);

    if (levelCompleteText != null)
    {
        levelCompleteText.gameObject.SetActive(true);
        levelCompleteText.text = "LEVEL COMPLETE";
    }
}


}
