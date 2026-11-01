using UnityEngine; 

public class ShowScore : MonoBehaviour
{
    public static int score = 0; // 'static' so that it can be referenced in ScoreChange.cs
    public GameObject scoreDisplay;

    // When game starts, score is always set to 0
    void Awake()
    {
        score = 0;
    }

    // Update the score in the UI
    void Update()
    {
        scoreDisplay.GetComponent<TMPro.TMP_Text>().text = "SCORE: " + score;
    }
}
