using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 20;
    public int currentHealth;

    public Image healthBarImage;
    public Sprite[] healthBarSprites;

    public GameObject gameOverPanel;

    private int bulletHitCount = 0;
    public int hitsPerHealthLoss = 2;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private bool isInvincible = false;

    public AudioClip loseSoundClip;
    private AudioSource loseSoundAudioSource;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        Debug.Log($"Player Health: {currentHealth}/{maxHealth}");
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        // Setup lose sound AudioSource
        loseSoundAudioSource = gameObject.AddComponent<AudioSource>();
        loseSoundAudioSource.clip = loseSoundClip;
        loseSoundAudioSource.loop = false;
        loseSoundAudioSource.volume = 0f;
    }

    public void SetInvincibility(bool value)
    {
        isInvincible = value;
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log($"Player Health: {currentHealth}/{maxHealth}");

        UpdateHealthUI();

        if (spriteRenderer != null)
            StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeEnemyBulletHit()
    {
        if (isInvincible) return;

        bulletHitCount++;
        if (bulletHitCount >= hitsPerHealthLoss)
        {
            bulletHitCount = 0;
            TakeDamage(1);
        }
    }

    void UpdateHealthUI()
    {
        if (healthBarSprites.Length == 0 || healthBarImage == null) return;

        float healthPercent = (float)currentHealth / maxHealth;
        int index = Mathf.RoundToInt(healthPercent * (healthBarSprites.Length - 1));
        index = (healthBarSprites.Length - 1) - index;

        healthBarImage.sprite = healthBarSprites[index];
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        Debug.Log("Player Dead!");
        Time.timeScale = 0f;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.SetActive(false);
        }

        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("EnemyBullet"))
        {
            bullet.SetActive(false);
        }

        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("PlayerBullet"))
        {
            bullet.SetActive(false);
        }

        GameObject player = GameObject.FindGameObjectWithTag("PlayerShip");
        if (player != null)
        {
            PlayerController controller = player.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.canShoot = false;
                controller.enabled = false;
            }
        }

        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss != null)
        {
            BossController bossController = boss.GetComponent<BossController>();
            if (bossController != null)
            {
                bossController.HandleGameOver();
            }
        }

        WaveSpawner spawner = FindObjectOfType<WaveSpawner>();
        if (spawner != null)
        {
            spawner.StopGameMusic();
        }

        // Play lose sound
        SoundManager.Instance.PlayLoseSound();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Disable player GameObject last
        gameObject.SetActive(false);
    }
}
