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
                SpawnWave(enemyPrefab, 10);
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
    float camHeight = Camera.main.orthographicSize;
    float camWidth = camHeight * Camera.main.aspect;
    float spawnX = Camera.main.transform.position.x + camWidth - 10f;

    // Define grid rows (Y positions)
    int rows = 7;
    float spacing = camHeight * 2f / (rows + 1); // full height divided evenly
    List<float> yPositions = new List<float>();

    for (int i = 1; i <= rows; i++)
    {
        float y = Camera.main.transform.position.y - camHeight + spacing * i;
        yPositions.Add(y);
    }

    // Shuffle Y positions to randomize row selection
    for (int i = 0; i < yPositions.Count; i++)
    {
        float temp = yPositions[i];
        int randomIndex = Random.Range(i, yPositions.Count);
        yPositions[i] = yPositions[randomIndex];
        yPositions[randomIndex] = temp;
    }

    // Spawn enemies in available rows
    for (int i = 0; i < count && i < yPositions.Count; i++)
    {
        Vector3 spawnPos = new Vector3(spawnX, yPositions[i], 0f);
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
        currentEnemies.Add(enemy);
    }

    waveInProgress = false;
}

}
