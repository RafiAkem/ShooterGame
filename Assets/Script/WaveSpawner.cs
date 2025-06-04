using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject bossPrefab;

    private List<GameObject> currentEnemies = new List<GameObject>();
    private int currentWave = 0;
    private bool waveInProgress = false;

    void Update()
    {
        currentEnemies.RemoveAll(e => e == null);

        if (!waveInProgress && currentEnemies.Count == 0)
        {
            currentWave++;
            StartWave(currentWave);
        }
    }

    void StartWave(int wave)
    {
        waveInProgress = true;

        switch (wave)
        {
            case 1:
                SpawnWave(enemyPrefab, 3);
                break;
            case 2:
                SpawnWave(enemyPrefab, 5);
                break;
            case 3:
                SpawnWave(bossPrefab, 1);
                break;
            default:
                Debug.Log("All waves complete!");
                break;
        }
    }

void SpawnWave(GameObject prefab, int count)
{
    float camHeight = Camera.main.orthographicSize;       // half vertical size
    float camWidth = camHeight * Camera.main.aspect;      // half horizontal size

    // Spawn X just outside the right edge of the camera view
    float spawnX = Camera.main.transform.position.x + camWidth - 10f;

    // Y range inside camera vertical bounds with margin
    float minY = Camera.main.transform.position.y - camHeight + 1f;
    float maxY = Camera.main.transform.position.y + camHeight - 1f;

    for (int i = 0; i < count; i++)
    {
        float spawnY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);

        Debug.Log($"Spawning enemy at {spawnPos} | Camera at {Camera.main.transform.position}");

        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
        currentEnemies.Add(enemy);
    }

    waveInProgress = false;
}

}
