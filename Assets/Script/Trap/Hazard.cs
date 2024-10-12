using UnityEngine;

public class Hazard : MonoBehaviour {
    public int damageAmount = 2;  // Amount of damage the hazard deals

    // This method is called when something enters the trigger collider
    private void OnTriggerEnter2D(Collider2D other) {
        // Try to get the Character component from the object that collided
        Character character = other.GetComponent<Character>();

        // If the object has a Character component, apply damage
        if (character != null) {
            character.TakeDamage(damageAmount);  // Polymorphic behavior
            Debug.Log($"{other.gameObject.name} touched the hazard and took damage!");
        }
    }
}
