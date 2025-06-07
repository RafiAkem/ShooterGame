using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 5f;
    public float waveAmplitude = 0.5f;
    public float waveFrequency = 5f;
    public float lifetime = 5f;

    private Vector3 startPos;
    private float spawnTime;

    void Start()
    {
        startPos = transform.position;
        spawnTime = Time.time;
        Destroy(gameObject, lifetime);
    }

void Update()
{
    float timeSinceSpawn = Time.time - spawnTime;

    // Move left while oscillating vertically around original Y
    float x = transform.position.x - speed * Time.deltaTime;
    float y = startPos.y + Mathf.Sin(timeSinceSpawn * waveFrequency) * waveAmplitude;

    transform.position = new Vector3(x, y, transform.position.z);
}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // You can call damage logic on player here
            Debug.Log("Boss bullet hit the player!");
            Destroy(gameObject);
        }
    }
}
