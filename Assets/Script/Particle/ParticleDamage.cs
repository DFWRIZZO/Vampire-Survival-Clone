using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    public int damage = 1;  // Set the amount of damage each particle should deal
    public LayerMask enemyLayer;  // Only affect enemies

    // Method to increase particle damage dynamically
    public void IncreaseDamage(int amount)
    {
        damage += amount;
        Debug.Log($"Particle damage increased! New damage: {damage}");
    }

    private void OnParticleCollision(GameObject other)
    {
        if (((1 << other.layer) & enemyLayer) != 0)
        {  // Check if the other object is on the enemy layer
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"{other.name} takes {damage} damage from particle collision!");
            }
        }
    }
}
