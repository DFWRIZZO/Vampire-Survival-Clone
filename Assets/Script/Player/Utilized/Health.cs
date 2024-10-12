using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour {
    public Image healthFillImage; // Assign this in the inspector with the image representing the health fill
    public Player player;

    void Start() {
        if (player != null && healthFillImage != null) {
            // Initialize the health bar fill amount to full
            healthFillImage.fillAmount = player.Health / player.maxHealth;
        }
    }

    void Update() {
        if (player != null && healthFillImage != null) {
            // Update the health bar fill amount based on player's current health
            healthFillImage.fillAmount = player.Health / player.maxHealth;
        }
    }
}
