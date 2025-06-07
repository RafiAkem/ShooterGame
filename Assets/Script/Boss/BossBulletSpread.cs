using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletSpread : MonoBehaviour
{
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Spread bullet hit player!");
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeEnemyBulletHit();
            }

            Destroy(gameObject);
        }
    }
}
