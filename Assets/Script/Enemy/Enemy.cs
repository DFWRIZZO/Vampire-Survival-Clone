using UnityEngine;
using TMPro;

public class Enemy : Character
{
    [SerializeField] private Transform player;
    public Animator animator;
    public GameObject damageIndicatorPrefab;  // Reference to the damage indicator prefab with TextMeshProUGUI
    private SpriteRenderer spriteRenderer;    // Reference to the SpriteRenderer component
    private bool isDead = false;              // Flag to track if the enemy is dead
    public static System.Action<Transform> OnEnemyDestroyed;  // Event to notify when an enemy is destroyed

    protected override void Start()
    {
        base.Start();
        health = 5;  // Set enemy-specific health
        spriteRenderer = GetComponent<SpriteRenderer>();  // Get the SpriteRenderer component for color changes
    }

    public override void TakeDamage(int damage)
    {
        if (isDead) return;  // If already dead, ignore any further damage

        health -= damage;
        ShowDamageIndicator(damage);  // Show the damage indicator
        StartCoroutine(FlashRed());   // Flash the sprite red

        if (health <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        isDead = true;  // Set the enemy as dead to avoid further interactions
        OnEnemyDestroyed?.Invoke(transform);  // Invoke the OnEnemyDestroyed event before destroying the object
        Destroy(gameObject);  // Destroy the enemy game object
    }

    // Method to instantiate and show the damage indicator
    private void ShowDamageIndicator(int damage)
    {
        if (damageIndicatorPrefab != null)
        {
            // Instantiate the damage indicator above the enemy
            GameObject indicator = Instantiate(damageIndicatorPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);

            // Use TextMeshProUGUI component to set the text
            TextMeshProUGUI tmp = indicator.GetComponentInChildren<TextMeshProUGUI>();  // Find the TextMeshProUGUI in the prefab
            if (tmp != null)
            {
                tmp.text = damage.ToString();
            }

            // Destroy the indicator after a short delay (e.g., 1 second)
            Destroy(indicator, 1f);
        }
    }

    // Coroutine to flash the enemy sprite red for a brief moment
    private System.Collections.IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            // Change the color to red
            spriteRenderer.color = Color.red;

            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            // Revert back to the original color
            spriteRenderer.color = Color.white;
        }
    }
}
