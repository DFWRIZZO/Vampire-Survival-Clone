using UnityEngine;

public class ExperiencePickup : MonoBehaviour
{
    public int expAmount = 20; // Amount of EXP this prefab gives
    public float moveSpeed = 5f; // Speed at which the prefab moves toward the player
    public float trackingRange = 5f; // Distance within which the prefab tracks the player
    public float lifeTime = 5f; // Time before the prefab is destroyed if not collected

    private Transform playerTransform;

    void Start()
    {
        // Find the player by tag and get its transform
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Automatically destroy this object after `lifeTime` seconds if not collected
        Invoke("DestroySelf", lifeTime);
    }

    void Update()
    {
        // If the player is within range, move toward the player
        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) <= trackingRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Get the PlayerLevel component and add experience
            PlayerLevel playerLevel = collision.GetComponent<PlayerLevel>();
            if (playerLevel != null)
            {
                playerLevel.GainExperience(expAmount);
            }

            // Destroy the EXP prefab after it has been collected
            Destroy(gameObject);
        }
    }

    // Destroy this GameObject if it exists during scene unload or application quit
    private void DestroySelf()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    // Cleanup when the scene is unloaded
    void OnDestroy()
    {
        CancelInvoke();  // Cancel the Invoke call to avoid lingering calls when destroyed
    }
}
