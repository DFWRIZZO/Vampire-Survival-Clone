using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerLevel : MonoBehaviour
{
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    public TextMeshProUGUI levelText;
    public Slider expSlider;
    public GameObject powerUpPanel;
    public PlayerAttack playerAttack;
    public Player player;
    public ParticleDamage particleDamage;  // Reference to the ParticleDamage script
    public event Action<int> OnLevelChanged;

    private bool hasBlockSkill = false;  // Track if block skill has been chosen

    void Start()
    {
        UpdateUI();
    }

    public void GainExperience(int amount)
    {
        currentExp += amount;
        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }

        UpdateUI();
    }

    void LevelUp()
    {
        currentExp -= expToNextLevel;
        currentLevel++;
        expToNextLevel += 50;

        // Scale power-up values every 50 levels
        if (currentLevel % 50 == 0)
        {
            ScalePowerUps();
        }

        if (currentLevel % 5 == 0)
        {
            ShowPowerUpSelection();
        }

        UpdateUI();
        OnLevelChanged?.Invoke(currentLevel);
    }

    void ScalePowerUps()
    {
        if (playerAttack != null)
        {
            playerAttack.critDamage *= 2;
            playerAttack.critChance *= 2;
            playerAttack.attackDamage *= 2;
        }
        if (player != null)
        {
            player.maxHealth *= 2;
            player.moveSpeed *= 2;
        }
    }

    void UpdateUI()
    {
        if (expSlider != null)
        {
            expSlider.maxValue = expToNextLevel;
            expSlider.value = currentExp;
        }

        if (levelText != null)
        {
            levelText.text = "Level " + currentLevel;
        }
    }

    void ShowPowerUpSelection()
    {
        Time.timeScale = 0f;
        powerUpPanel.SetActive(true);

        PowerUpPanel panelScript = powerUpPanel.GetComponent<PowerUpPanel>();
        if (panelScript != null)
        {
            panelScript.DisplayRandomPowerUps(hasBlockSkill);
        }
    }

    public void ApplyPowerUp(string powerUpType)
{
    switch (powerUpType)
    {
        case "IncreaseCritChance":
            if (playerAttack != null)
            {
                playerAttack.critChance += 0.1f;
            }
            break;

        case "IncreaseHealth":
            if (player != null)
            {
                player.maxHealth += 50;
                player.Health = player.maxHealth;
            }
            break;

        case "IncreaseDamage":
            if (playerAttack != null)
            {
                playerAttack.attackDamage += 1;
            }
            break;

        case "IncreaseMovementSpeed":
            if (player != null)
            {
                player.moveSpeed += 1f;
            }
            break;

        case "IncreaseParticleCount":
            if (playerAttack != null)
            {
                IncreaseParticleBurstCount(playerAttack.critEffect, 2);
            }
            break;

        case "IncreaseParticleDamage":
            if (particleDamage != null)  // Ensure the reference is assigned
            {
                particleDamage.damage += 1;  // Increase the damage property, not the reference itself
            }
            break;

        case "IncreaseCritDamage":
            if (playerAttack != null)
            {
                playerAttack.critDamage += 1;
            }
            break;

        case "BlockSkill":
            if (player != null)
            {
                player.EnableBlockSkill(); // Grant the player access to block
                hasBlockSkill = true;  // Track that the block skill has been chosen
            }
            break;

        default:
            break;
    }

    powerUpPanel.SetActive(false);
    Time.timeScale = 1f;
}


    private void IncreaseParticleBurstCount(ParticleSystem particleSystem, int additionalCount)
    {
        var emission = particleSystem.emission;
        ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];
        emission.GetBursts(bursts);

        if (bursts.Length > 0)
        {
            bursts[0].count = bursts[0].count.constant + additionalCount;
            emission.SetBursts(bursts);
        }
        else
        {
            ParticleSystem.Burst newBurst = new ParticleSystem.Burst(0.0f, additionalCount);
            emission.SetBursts(new ParticleSystem.Burst[] { newBurst });
        }
    }
}
