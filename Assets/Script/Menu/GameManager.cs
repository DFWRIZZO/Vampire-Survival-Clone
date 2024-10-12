using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI; // Reference to the Game Over UI
    public TextMeshProUGUI timerText; // Reference to the timer display text during the game
    public TextMeshProUGUI highScoreText; // Reference to the high score display text on the main menu
    private float gameTime = 0f; // Track the current game time
    private bool isGameActive = true; // Check if the game is currently active

    private const string HighScoreKey = "LongestSurvivalTime"; // Key for storing high score in PlayerPrefs

    void Start()
    {
        // Initialize high score display on the main menu
        if (highScoreText != null)
        {
            float highScore = PlayerPrefs.GetFloat(HighScoreKey, 0f);
            highScoreText.text = "Longest Survival: " + FormatTime(highScore);
        }

        // Ensure Time.timeScale is set to 1 when starting the game to avoid being stuck at zero time scale
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (isGameActive)
        {
            gameTime += Time.deltaTime;
            if (timerText != null)
            {
                timerText.text = "Time: " + FormatTime(gameTime);
            }
        }
    }

    // Method to handle game over event
    public void TriggerGameOver()
    {
        isGameActive = false;

        // Destroy all EXP objects when the game ends
        DestroyAllEXPObjects();

        gameOverUI.SetActive(true); // Show Game Over screen
        Time.timeScale = 0f; // Pause the game

        // Save the high score if the player survived longer than the current high score
        float highScore = PlayerPrefs.GetFloat(HighScoreKey, 0f);
        if (gameTime > highScore)
        {
            PlayerPrefs.SetFloat(HighScoreKey, gameTime);
            PlayerPrefs.Save();
        }

        Debug.Log("Game Over triggered. High Score: " + FormatTime(highScore));
    }

    // Method to restart the game and reset all necessary parameters
    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    // Method to navigate back to the main menu
    public void MainMenu()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene(0); // Load the main menu scene (assumed to be at build index 0)
    }

    // Method to quit the game application
    public void QuitGame()
    {
        Application.Quit();
    }

    // Method to destroy all active EXP objects in the scene
    private void DestroyAllEXPObjects()
    {
        GameObject[] expObjects = GameObject.FindGameObjectsWithTag("EXP");
        Debug.Log("Number of EXP objects found: " + expObjects.Length);

        foreach (GameObject exp in expObjects)
        {
            Debug.Log("Destroying EXP object: " + exp.name);
            Destroy(exp);
        }
    }

    // Utility method to format time into a readable string
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Automatically destroy lingering EXP objects when the scene is unloaded
    void OnDestroy()
    {
        // Ensure any remaining EXP objects are cleaned up when the GameManager is destroyed (e.g., on scene unload)
        Debug.Log("GameManager is being destroyed, cleaning up all EXP objects.");
        DestroyAllEXPObjects();
    }
}
