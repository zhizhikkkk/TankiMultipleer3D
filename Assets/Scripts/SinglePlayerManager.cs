using UnityEngine;
using System.Collections;

public class SinglePlayerManager : MonoBehaviour
{
    public static SinglePlayerManager Instance;

    public GameObject playerTankPrefab;
    public GameObject enemyTankPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartGame()
    {
        GameManager.Instance.gameUI.SetActive(true);
        SpawnPlayer();
        StartCoroutine(SpawnEnemies());
    }

    private void SpawnPlayer()
    {
        Instantiate(playerTankPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    private IEnumerator SpawnEnemies()
    {
        int waveSize = 2;
        int increaseRate = 1;
        float waveTimer = 20f;
        float spawnInterval = 5f;
        float elapsedTime = 0f;

        for (int i = 0; i < waveSize; i++)
        {
            Instantiate(enemyTankPrefab, GetSpawnPositionNearPlayer(), Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }

        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            for (int i = 0; i < waveSize; i++)
            {
                Instantiate(enemyTankPrefab, GetRandomSpawnPosition(), Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
            }

            elapsedTime += spawnInterval;

            if (elapsedTime >= waveTimer)
            {
                waveSize += increaseRate;
                elapsedTime = 0f;
            }
        }
    }

    private Vector3 GetSpawnPositionNearPlayer()
    {
        return new Vector3(Random.Range(3f, 6f), 0, Random.Range(3f, 6f));
    }

    private Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
    }
}
