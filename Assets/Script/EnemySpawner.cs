using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public float spawnYMin = -3f;
    public float spawnYMax = 3f;

    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        float camHeight = Camera.main.orthographicSize * 2f;
        float camWidth = camHeight * Camera.main.aspect;
        Vector3 spawnPos = new Vector3(Camera.main.transform.position.x + camWidth / 2 - 5f,
                                       Random.Range(spawnYMin, spawnYMax),
                                       0f);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
