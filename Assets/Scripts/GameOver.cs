using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject fadeOut;
    public GameObject gameOverUI;
    public GameObject restartButton;

    // If Player dies, show game over screen
    public void TriggerGameOver()
    {
        fadeOut.SetActive(true);
        gameOverUI.SetActive(true);
        restartButton.SetActive(true);
    }

    // If User clicks on the Restart button, go back to Main Scene
    public void RestartGame()
    {
        SceneManager.LoadScene("Main Scene");
    }
}