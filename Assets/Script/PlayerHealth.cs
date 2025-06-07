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

    void Start() {
        currentHealth = maxHealth;
        UpdateHealthUI();
        Debug.Log($"Player Health: {currentHealth}/{maxHealth}");
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Get the SpriteRenderer and store original color
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log($"Player Health: {currentHealth}/{maxHealth}");

        UpdateHealthUI();

        // Flash red when taking damage
        if (spriteRenderer != null)
            StartCoroutine(FlashRed());

        if (currentHealth <= 0) {
            Die();
        }
    }

    public void TakeEnemyBulletHit() {
        bulletHitCount++;
        if (bulletHitCount >= hitsPerHealthLoss) {
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

        //hidden player object
        gameObject.SetActive(false);


        // Disable all enemies
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.SetActive(false);
        }

        //if there's boss

        // Disable all bullets
        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("EnemyBullet"))
        {
            bullet.SetActive(false);
        }

        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("PlayerBullet"))
        {
            bullet.SetActive(false);
        }

        // Disable player shooting and movement
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

        // Show Game Over UI
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}
