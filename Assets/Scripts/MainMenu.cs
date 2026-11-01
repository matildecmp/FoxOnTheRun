using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject fadeOut;

    // When User presses Start button in Main Menu, Instructions scene is loaded
    public void StartGame()
    {
        SceneManager.LoadScene("Instructions");
    }

    // When User presses Play button in Instructions scene, Main Scene is loaded after a fade out effect
    public void PlayGame()
    {
        StartCoroutine(PlayButton());
    }

    IEnumerator PlayButton()
    {
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Main Scene"); 
    }
}

