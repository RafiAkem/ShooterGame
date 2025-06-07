using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public GameObject StageCompleteUI;
  

    public Transform enemyContainer;

    public TMP_Text waveAnnouncementText;
    public float waveAnnouncementDuration = 2f;
    public float timeBetweenColumns = 0.5f;
    public float delayBetweenWaves = 3f;

    private List<GameObject> currentEnemies = new List<GameObject>();
    private int currentWave = 0;
    private bool waveInProgress = false;

    public AudioClip bossIncomingClip;
    private AudioSource audioSource;
    public AudioClip normalWaveMusicClip;
    public AudioClip bossFightMusicClip;

    private AudioSource musicAudioSource;

    void Start()
    {
        //StageCompleteUI.SetActive(false);
        if (StageCompleteUI != null) { 
        StageCompleteUI.SetActive(false);
        }

        currentWave = 0;
        currentEnemies.Clear();
        audioSource = GetComponent<AudioSource>();

        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.loop = true;

        if (normalWaveMusicClip != null)
    {
        musicAudioSource.clip = normalWaveMusicClip;
        musicAudioSource.Play();
    }

        StartCoroutine(StartWave(currentWave));
    }
    void Update()
    {
        currentEnemies.RemoveAll(e => e == null);

        if (!waveInProgress && currentEnemies.Count == 0)
        {
            currentWave++;
            StartCoroutine(StartWave(currentWave));
        }
    }

    IEnumerator StartWave(int wave)
    {
        waveInProgress = true;

        if (wave == 1 || wave == 2)
        {
            if (waveAnnouncementText != null)
            {
                waveAnnouncementText.text = "WAVE " + wave +" INCOMING";
                waveAnnouncementText.gameObject.SetActive(true);
                yield return StartCoroutine(FlashText(waveAnnouncementText, waveAnnouncementDuration, 0.3f));
                waveAnnouncementText.gameObject.SetActive(false);
            }
        }

        switch (wave)
        {
            case 1:
                yield return StartCoroutine(SpawnWave(enemyPrefab, 3));
                break;
            case 2:
                yield return StartCoroutine(SpawnWave(enemyPrefab, 5));
                break;
            case 3:
                yield return StartCoroutine(StartBossWave(bossPrefab));
                break;
            case 4:
                //delay 4 detik
                yield return new WaitForSeconds(4);
                if (StageCompleteUI != null)
                {
                    StageCompleteUI.SetActive(true);
                }
                break;
            default:
                Debug.Log("All waves complete!");
                waveInProgress = false;
                yield break;
        }

        waveInProgress = false;
        
    }

    IEnumerator FlashText(TMP_Text text, float duration, float flashInterval)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            text.enabled = !text.enabled;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }
        text.enabled = true;
    }

    IEnumerator SpawnWave(GameObject prefab, int totalColumns)
    {
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
        float enemyWidth = sr != null ? sr.bounds.size.x : 1f;
        float enemyHeight = sr != null ? sr.bounds.size.y : 1f;

        float spacingX = enemyWidth + 1f;
        float spacingY = enemyHeight + 0.5f;

        float pyramidWidth = totalColumns * spacingX;
        float leftEdge = Camera.main.transform.position.x + camWidth - pyramidWidth + (spacingX / 2f);
        float centerY = Camera.main.transform.position.y;

        for (int col = 0; col < totalColumns; col++)
        {
            int enemiesInColumn = col + 1;
            float columnHeight = (enemiesInColumn - 1) * spacingY;
            float startY = centerY - columnHeight / 2f;
            float x = leftEdge + col * spacingX;

            for (int row = 0; row < enemiesInColumn; row++)
            {
                float y = startY + row * spacingY;
                Vector3 spawnPos = new Vector3(x, y, 0f);

                GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity, enemyContainer);
                enemy.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                currentEnemies.Add(enemy);
            }

            yield return new WaitForSeconds(timeBetweenColumns);
        }
    }

    IEnumerator StartBossWave(GameObject boss)
    {
        if (waveAnnouncementText != null)
        {
            waveAnnouncementText.text = "BOSS INCOMING !";
            waveAnnouncementText.gameObject.SetActive(true);

        if (audioSource != null && bossIncomingClip != null)
        {
            audioSource.PlayOneShot(bossIncomingClip);
        }

        // Switch music
        if (musicAudioSource != null && bossFightMusicClip != null)
        {
            musicAudioSource.Stop();
            musicAudioSource.clip = bossFightMusicClip;
            musicAudioSource.Play();
        }

            yield return StartCoroutine(FlashText(waveAnnouncementText, 3f, 0.5f));
            waveAnnouncementText.gameObject.SetActive(false);
        }

        waveInProgress = true;

        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        Vector3 bossSpawnPos = new Vector3(Camera.main.transform.position.x + camWidth + 2f, Camera.main.transform.position.y, 0f);
        GameObject bossInstance = Instantiate(boss, bossSpawnPos, Quaternion.identity, enemyContainer);
        // bossInstance.transform.localScale = Vector3.one;
        currentEnemies.Add(bossInstance);

        yield return StartCoroutine(BossEntrance(bossInstance));

        while (bossInstance != null)
        {
            yield return null;
        }

        waveInProgress = false;

    }

    IEnumerator BossEntrance(GameObject bossInstance)
    {
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;
        float targetX = Camera.main.transform.position.x + camWidth - 3f;
        float moveSpeed = 3f;

        while (bossInstance != null && bossInstance.transform.position.x > targetX)
        {
            bossInstance.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }
}