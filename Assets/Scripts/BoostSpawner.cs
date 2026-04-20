using System.Collections;
using UnityEngine;

public class BoostSpawner : MonoBehaviour
{
    [SerializeField] private GameObject boostPrefab;
    [SerializeField] private float maxSpawnTime = 20.0f;
    [SerializeField] private float minSpawnTime = 5.0f;
    [SerializeField] private float minRangeFromPlayer = 5.0f;
    [SerializeField] private float maxRangeFromPlayer = 10.0f;
    [SerializeField] private LayerMask roadLayerMask;
    
    private float currentInterval = 10.0f;
    PlayerController player;
    
    void Start()
    {
        player = PlayerController.Instance; // optimized
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (!GameManager.isGameOver)
        {
            currentInterval = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(currentInterval);
            
            bool validPosition = false;
            int attempts = 0; // optimized
            int maxAttempts = 10; // optimized
            
            while (!validPosition && attempts < maxAttempts) // optimized
            {
                attempts++; // optimized
                
                float range = Random.Range(minRangeFromPlayer, maxRangeFromPlayer);
            
                Vector3 desiredSpawnPosition = player.transform.position + player.transform.forward * range + Vector3.up * 100f;

                if (Physics.Raycast(desiredSpawnPosition, Vector3.down, out RaycastHit hit, Mathf.Infinity, roadLayerMask))
                {
                    validPosition = true;
                    Vector3 spawnPosition = hit.point + Vector3.up * 0.5f + Vector3.right * Random.value * 2.0f;
                    Instantiate(boostPrefab,  spawnPosition, Quaternion.identity);
                }
                
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
