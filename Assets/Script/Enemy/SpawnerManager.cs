using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public GameObject enemySpawnerPrefab; // Prefab of the EnemySpawner
    public Transform[] initialSpawnPoints; // Initial spawn points for the first spawner
    public float timeToAddSpawner = 60f; // Time interval to add new spawners (in seconds)
    public Vector2 spawnAreaSize = new Vector2(50, 50); // Size of the area in which to add new spawners

    private List<GameObject> spawners = new List<GameObject>(); // Track all active spawners
    private float elapsedTime = 0f; // Track elapsed time

    void Start()
    {
        // Create the first spawner with the initial spawn points
        CreateSpawner(initialSpawnPoints);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Add new spawner at set intervals
        if (elapsedTime >= timeToAddSpawner)
        {
            elapsedTime = 0f; // Reset the timer
            CreateSpawnerAtRandomLocation();
        }
    }

    // Method to create a new spawner at a specific set of spawn points
    void CreateSpawner(Transform[] spawnPoints)
    {
        GameObject newSpawner = Instantiate(enemySpawnerPrefab, Vector3.zero, Quaternion.identity);
        EnemySpawner spawnerScript = newSpawner.GetComponent<EnemySpawner>();

        if (spawnerScript != null)
        {
            spawnerScript.spawnPoints = spawnPoints; // Assign spawn points to the new spawner
        }

        spawners.Add(newSpawner); // Keep track of all active spawners
    }

    // Method to create a new spawner at a random location
    void CreateSpawnerAtRandomLocation()
    {
        // Generate a random position within a defined area
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnAreaSize.x, spawnAreaSize.x),
            Random.Range(-spawnAreaSize.y, spawnAreaSize.y),
            0f
        );

        // Instantiate a new spawner at the random position
        GameObject newSpawner = Instantiate(enemySpawnerPrefab, randomPosition, Quaternion.identity);
        spawners.Add(newSpawner); // Keep track of all active spawners
    }
}
