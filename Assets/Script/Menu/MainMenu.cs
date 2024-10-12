using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Player player; // Drag the Player object in the Inspector

    public void PlayGame() {
        Time.timeScale = 1f;  // Reset time scale if it was changed
        player.ResetPlayer();
        SceneManager.LoadSceneAsync(1);
      
    }
    public void QuitGame() { 
        Application.Quit();
    }
}
