using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public Transform[] spawnPoints; // Array of spawn points
    public float minSpawnInterval = 2f; // Minimum time between spawns
    public float maxSpawnInterval = 5f; // Maximum time between spawns
    public int minEnemiesPerWave = 2;  // Minimum number of enemies to spawn per wave
    public int maxEnemiesPerWave = 5;  // Maximum number of enemies to spawn per wave
    public PlayerLevel playerLevel;    // Reference to the player's level script

    private float timeSinceLastSpawn;
    private float currentSpawnInterval; // Current random spawn interval
    private bool instantSpawnTriggered = false;  // Check if instant spawn was triggered

    void Start()
    {
        timeSinceLastSpawn = 0f;
        SetRandomSpawnInterval(); // Set the first random interval

        // Subscribe to the player's level change event
        if (playerLevel != null)
        {
            playerLevel.OnLevelChanged += HandleLevelChanged;
        }
    }

    void Update()
    {
        // Only run update logic if spawn points are assigned and instant spawn is not triggered
        if (spawnPoints == null || spawnPoints.Length == 0 || instantSpawnTriggered)
        {
            return; // Prevent the Update from running if there are no spawn points or level 50 is reached
        }

        timeSinceLastSpawn += Time.deltaTime;

        // Check if enough time has passed to spawn the next wave
        if (timeSinceLastSpawn >= currentSpawnInterval)
        {
            SpawnEnemies();
            timeSinceLastSpawn = 0f;

            // Set a new random interval for the next spawn
            SetRandomSpawnInterval();
        }
    }

    // Method to handle level changes
    void HandleLevelChanged(int newLevel)
    {
        if (newLevel == 50 && !instantSpawnTriggered)
        {
            // If the player reaches level 50, spawn enemies instantly
            instantSpawnTriggered = true;
            SpawnEnemies();
        }
    }

    // Method to spawn a group of enemies at random spawn points
    void SpawnEnemies()
    {
        // Determine the number of enemies to spawn this wave
        int enemiesToSpawn = Random.Range(minEnemiesPerWave, maxEnemiesPerWave + 1);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Pick a random spawn point for each enemy
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];

            // Spawn the enemy at the selected spawn point
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        Debug.Log("Enemies spawned!");
    }

    // Method to set a new random spawn interval
    void SetRandomSpawnInterval()
    {
        currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent errors when the object is destroyed
        if (playerLevel != null)
        {
            playerLevel.OnLevelChanged -= HandleLevelChanged;
        }
    }
}
