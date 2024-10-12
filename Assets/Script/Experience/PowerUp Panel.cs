using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpPanel : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;
    public TextMeshProUGUI button1Text;
    public TextMeshProUGUI button2Text;
    public TextMeshProUGUI button3Text;
    private PlayerLevel playerLevel;

    // Updated pool with descriptive texts and corresponding types
    private readonly string[] powerUpPool = {
        "Increase Crit Chance by 0.1",
        "Increase Health by 50",
        "Increase Damage by 1",
        "Increase Movement Speed by 1",
        "Increase Particle Emission Count by 2",
        "Increase Particle Damage by 1",
        "Increase Crit Damage by 1",
        "Block Skill" // Block skill power-up
    };

    private readonly string[] powerUpTypes = {
        "IncreaseCritChance",
        "IncreaseHealth",
        "IncreaseDamage",
        "IncreaseMovementSpeed",
        "IncreaseParticleCount",
        "IncreaseParticleDamage",
        "IncreaseCritDamage",
        "BlockSkill"
    };

    private string powerUpType1, powerUpType2, powerUpType3;

    void Start()
    {
        playerLevel = FindObjectOfType<PlayerLevel>();

        button1.onClick.AddListener(() => playerLevel.ApplyPowerUp(powerUpType1));
        button2.onClick.AddListener(() => playerLevel.ApplyPowerUp(powerUpType2));
        button3.onClick.AddListener(() => playerLevel.ApplyPowerUp(powerUpType3));
    }

    public void DisplayRandomPowerUps(bool blockSkillSelected)
{
    int index1 = Random.Range(0, powerUpPool.Length);
    int index2 = Random.Range(0, powerUpPool.Length);
    int index3 = Random.Range(0, powerUpPool.Length);

    while (index2 == index1) index2 = Random.Range(0, powerUpPool.Length);
    while (index3 == index1 || index3 == index2) index3 = Random.Range(0, powerUpPool.Length);

    if (blockSkillSelected)
    {
        while (powerUpTypes[index1] == "BlockSkill") index1 = Random.Range(0, powerUpPool.Length);
        while (powerUpTypes[index2] == "BlockSkill") index2 = Random.Range(0, powerUpPool.Length);
        while (powerUpTypes[index3] == "BlockSkill") index3 = Random.Range(0, powerUpPool.Length);
    }

    button1Text.text = powerUpPool[index1];
    button2Text.text = powerUpPool[index2];
    button3Text.text = powerUpPool[index3];

    powerUpType1 = powerUpTypes[index1];
    powerUpType2 = powerUpTypes[index2];
    powerUpType3 = powerUpTypes[index3];

    SetButtonColor(button1, powerUpType1);
    SetButtonColor(button2, powerUpType2);
    SetButtonColor(button3, powerUpType3);
}


    private void SetButtonColor(Button button, string powerUpType)
    {
        if (powerUpType == "BlockSkill")
        {
            button.GetComponent<Image>().color = Color.red;  // Dark Red
            button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        else if (powerUpType.Contains("Crit") || powerUpType.Contains("Particle"))
        {
            button.GetComponent<Image>().color = Color.yellow;  // Yellow for rare types
            button.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
        else
        {
            button.GetComponent<Image>().color = Color.white;  // Default white
            button.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
    }
}
