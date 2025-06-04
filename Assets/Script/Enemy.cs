using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public float moveRange = 10f;
    private float startY;
    private bool movingUp = true;

    void Start()
    {
        startY = transform.position.y;
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
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("PlayerBullet"))
    {
        Destroy(other.gameObject);   // destroy the bullet
        Destroy(gameObject);         // destroy the enemy
    }
}
}
