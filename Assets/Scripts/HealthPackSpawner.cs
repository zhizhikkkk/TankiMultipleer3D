using UnityEngine;
using System.Collections;

public class HealthPackSpawner : MonoBehaviour
{
    public GameObject healthPackPrefab;
    public float spawnInterval = 15f; 
    private float spawnRange = 5f; 

    void Start()
    {
        StartCoroutine(SpawnHealthPacks());
    }

    private IEnumerator SpawnHealthPacks()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                0.5f, 
                Random.Range(-spawnRange, spawnRange)
            );

            Instantiate(healthPackPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
