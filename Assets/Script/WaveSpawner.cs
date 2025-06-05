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

void SpawnWave(GameObject prefab, int totalColumns)
{
    float camHeight = Camera.main.orthographicSize;
    float camWidth = camHeight * Camera.main.aspect;

    SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
    float enemyWidth = sr != null ? sr.bounds.size.x : 1f;
    float enemyHeight = sr != null ? sr.bounds.size.y : 1f;

    float spacingX = enemyWidth + 1f;
    float spacingY = enemyHeight + 0.5f;

    // Calculate total width of the pyramid
    float pyramidWidth = totalColumns * spacingX;

    // We want the left edge of the pyramid near the right edge of the camera
    float leftEdge = Camera.main.transform.position.x + camWidth - pyramidWidth + (spacingX / 2f);

    float centerY = Camera.main.transform.position.y;

    for (int col = 0; col < totalColumns; col++)
    {
        int enemiesInColumn = col + 1;  // 1, 2, 3, ...

        float columnHeight = (enemiesInColumn - 1) * spacingY;
        float startY = centerY - columnHeight / 2f;

        // Spawn columns from left to right
        float x = leftEdge + col * spacingX;

        for (int row = 0; row < enemiesInColumn; row++)
        {
            float y = startY + row * spacingY;
            Vector3 spawnPos = new Vector3(x, y, 0f);

            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
            currentEnemies.Add(enemy);
        }
    }

    waveInProgress = false;
}









}
