using UnityEngine;
using System.Collections;

public class TankSpawner : MonoBehaviour
{
    public static TankSpawner Instance;

    public GameObject playerTankPrefab;
    public GameObject enemyTankPrefab;

    void Awake()
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
        Vector3 spawnPosition = new Vector3(0, 10, 0);
        RaycastHit hit;

        if (Physics.Raycast(spawnPosition, Vector3.down, out hit, 20f))
        {
            spawnPosition = hit.point + Vector3.up * 0.1f;
        }

        GameObject player = Instantiate(playerTankPrefab, spawnPosition, Quaternion.identity);
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
            Instantiate(enemyTankPrefab, GetSpawnPositionAroundPlayer(), Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }

        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            for (int i = 0; i < waveSize; i++)
            {
                Instantiate(enemyTankPrefab, GetSpawnPositionAroundPlayer(), Quaternion.identity);
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

    private Vector3 GetSpawnPositionAroundPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Tank");
        if (!player) return GetRandomSpawnPosition();

        Vector3 playerPos = player.transform.position;

        float radius = Random.Range(1f, 10f);
        float angle = Random.Range(0f, 360f); 

        Vector3 offset = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
            10f, 
            Mathf.Sin(angle * Mathf.Deg2Rad) * radius
        );

        Vector3 spawnPos = playerPos + offset;

        RaycastHit hit;
        if (Physics.Raycast(spawnPos, Vector3.down, out hit, 20f))
        {
            spawnPos = hit.point + Vector3.up * 0.1f; 
        }

        return spawnPos;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
    }
}
