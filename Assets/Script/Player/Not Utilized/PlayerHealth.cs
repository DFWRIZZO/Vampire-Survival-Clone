using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    private int health = 5;  // private variable

    public void TakeDamage(int damage) {
        health -= damage;
        Debug.Log("Player took damage! Remaining health: " + health);

        if (health <= 0) {
            Die();
        }
    }

    public int GetHealth()  // Encapsulation: public method to get health
    {
        return health;
    }

    private void Die() {
        Debug.Log("Player died!");
        FindObjectOfType<GameManager>().TriggerGameOver(); // Trigger Game Over
        Destroy(gameObject);
    }
    
}
