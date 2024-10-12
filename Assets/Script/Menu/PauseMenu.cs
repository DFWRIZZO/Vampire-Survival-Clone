using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    public GameObject pauseMenuUI; // Assign in the Inspector
    private bool isPaused = false;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) // Toggle pause when pressing Escape
        {
            if (isPaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }

    public void ResumeGame() {
        pauseMenuUI.SetActive(false); // Hide the pause menu UI
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
    }

    public void PauseGame() {
        pauseMenuUI.SetActive(true); // Show the pause menu UI
        Time.timeScale = 0f; // Freeze game time
        isPaused = true;
    }

    public void RestartGame() {
        Time.timeScale = 1f; // Make sure the game time is running
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart the current scene
    }

    public void QuitGame() {
        Debug.Log("Quitting Game...");
        Application.Quit(); // Quit the game
    }
}
