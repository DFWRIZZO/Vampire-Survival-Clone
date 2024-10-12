using UnityEngine;
using System.Collections.Generic;

public class EnemyIndicator : MonoBehaviour
{
    [SerializeField] private GameObject indicatorPrefab;  // Reference to the indicator prefab
    [SerializeField] private float circleRadius = 2f;  // Radius of the circle around the player for indicator removal
    [SerializeField] private LayerMask enemyLayer;  // Define the enemy layer
    [SerializeField] private float detectionRadius = 10f;  // Radius for detecting enemies
    [SerializeField] private float deleteRange = 15f;  // Range for deleting indicators if no enemies/EXP present
    [SerializeField] private float rotationOffset = 90f;  // Offset to correct the indicator's orientation

    private Dictionary<Transform, GameObject> activeIndicators = new Dictionary<Transform, GameObject>();  // Track active indicators
    private Transform playerTransform;

    void Start()
    {
        playerTransform = transform;  // Get the player's transform
        Enemy.OnEnemyDestroyed += RemoveIndicator;  // Subscribe to enemy destruction event
    }

    void OnDestroy()
    {
        Enemy.OnEnemyDestroyed -= RemoveIndicator;  // Unsubscribe from the event to avoid memory leaks
    }

    void Update()
    {
        // Remove indicators for enemies that are within the circle radius
        List<Transform> enemiesToRemove = new List<Transform>();

        foreach (KeyValuePair<Transform, GameObject> pair in activeIndicators)
        {
            float distanceToEnemy = Vector3.Distance(playerTransform.position, pair.Key.position);
            if (distanceToEnemy <= circleRadius)
            {
                enemiesToRemove.Add(pair.Key);  // Mark this indicator for removal
            }
        }

        // Remove all marked indicators
        foreach (Transform enemy in enemiesToRemove)
        {
            RemoveIndicator(enemy);
        }

        // Detect new enemies and create indicators if they are outside the circle radius
        Collider2D[] detectedEnemies = Physics2D.OverlapCircleAll(playerTransform.position, detectionRadius, enemyLayer);

        foreach (Collider2D enemyCollider in detectedEnemies)
        {
            Transform enemyTransform = enemyCollider.transform;
            float distanceToEnemy = Vector3.Distance(playerTransform.position, enemyTransform.position);

            // Only create indicators for enemies outside the circle radius
            if (!activeIndicators.ContainsKey(enemyTransform) && distanceToEnemy > circleRadius)
            {
                CreateIndicator(enemyTransform);
            }
        }

        // Check if there are any "Enemy" or "EXP" tags within the delete range
        bool anyEnemiesOrExpNearby = CheckForEnemiesOrExpNearby();

        // If no enemies or EXP are within range, destroy indicators outside the delete range
        if (!anyEnemiesOrExpNearby)
        {
            DestroyIndicatorsOutsideDeleteRange();
        }
    }

    // Create an indicator for a specific enemy
    private void CreateIndicator(Transform enemyTransform)
    {
        // Instantiate the indicator prefab as a child of the player for easier positioning
        GameObject indicator = Instantiate(indicatorPrefab);
        indicator.transform.SetParent(playerTransform);  // Set parent to keep it organized under the player

        // Calculate direction from the player to the enemy
        Vector3 directionToEnemy = (enemyTransform.position - playerTransform.position).normalized;

        // Calculate the position of the indicator on the circle around the player
        Vector3 indicatorPosition = playerTransform.position + directionToEnemy * circleRadius;
        indicator.transform.position = indicatorPosition;

        // Correct the rotation so that the indicator points towards the enemy
        float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg + rotationOffset;
        indicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Store the indicator along with its enemy reference
        activeIndicators[enemyTransform] = indicator;
    }

    // Check if there are any "Enemy" or "EXP" tags within the new detection range
    private bool CheckForEnemiesOrExpNearby()
    {
        // Detect all objects with either "Enemy" or "EXP" tags within the delete range
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(playerTransform.position, deleteRange);

        // Iterate through detected objects and check if they have the desired tags
        foreach (Collider2D col in detectedObjects)
        {
            if (col.CompareTag("Enemy") || col.CompareTag("EXP"))
            {
                return true;  // Return true if any object with the "Enemy" or "EXP" tag is found
            }
        }

        return false;  // Return false if none are found
    }

    // Remove all indicators that are outside the delete range
    private void DestroyIndicatorsOutsideDeleteRange()
    {
        List<Transform> indicatorsToRemove = new List<Transform>();

        // Find all indicators outside the delete range
        foreach (KeyValuePair<Transform, GameObject> pair in activeIndicators)
        {
            float distanceToEnemy = Vector3.Distance(playerTransform.position, pair.Key.position);
            if (distanceToEnemy > deleteRange)
            {
                indicatorsToRemove.Add(pair.Key);
            }
        }

        // Remove all marked indicators
        foreach (Transform enemy in indicatorsToRemove)
        {
            RemoveIndicator(enemy);
        }
    }

    // Remove an indicator for a given enemy
    private void RemoveIndicator(Transform enemyTransform)
    {
        if (activeIndicators.ContainsKey(enemyTransform))
        {
            Destroy(activeIndicators[enemyTransform]);
            activeIndicators.Remove(enemyTransform);
        }
    }

    // Visualize the detection radius, orbit circle, and delete range in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);  // Draw the enemy detection radius

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, circleRadius);  // Draw the indicator's circle radius

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, deleteRange);  // Draw the new delete range
    }
}
