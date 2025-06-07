using System.Collections;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float speed = 30f;
    public float rotateSpeed = 100f;
    public int health = 3;
    public int damageToPlayer = 2;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

void Start()
{
    rb = GetComponent<Rigidbody2D>();
    player = GameObject.FindGameObjectWithTag("PlayerShip").transform;
    spriteRenderer = GetComponent<SpriteRenderer>();
    if (spriteRenderer != null)
        originalColor = spriteRenderer.color;

    if (player != null)
    {
        Vector2 directionToPlayer = (Vector2)(player.position - transform.position);
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}

void FixedUpdate()
{
    if (player == null) return;

    Vector2 direction = ((Vector2)player.position - rb.position).normalized;
    float angleToPlayer = Vector2.Angle(transform.right, direction);

    if (angleToPlayer < 90f)
    {
        float rotateAmount = Vector3.Cross(direction, transform.right).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;
    }
    else
    {
        rb.angularVelocity = 0;
    }

    rb.velocity = transform.right * speed;
}

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (spriteRenderer != null)
            StartCoroutine(FlashRed());

        if (health <= 0)
            Destroy(gameObject);
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject);
            TakeDamage(1);
        }
        else if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(damageToPlayer);

            Destroy(gameObject);
        }
    }
}
