using UnityEngine;
using System.Collections;

public class AmmoSpawner : MonoBehaviour
{
    [SerializeField]private GameObject ammoBoxPrefab;
    [SerializeField]private float spawnInterval = 10f; 

    private float spawnRange = 10f; 

    void Start()
    {
        StartCoroutine(SpawnAmmoBoxes());
    }

    private IEnumerator SpawnAmmoBoxes()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                0.5f, 
                Random.Range(-spawnRange, spawnRange)
            );

            Instantiate(ammoBoxPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
