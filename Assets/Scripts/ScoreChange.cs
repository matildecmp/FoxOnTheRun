using UnityEngine;

public class ScoreChange : MonoBehaviour
{
    public GameObject ptsAlertPrefab;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.name.Contains("Golden Apple"))
            {
                ShowScore.score += 5;
                Canvas UICanvas = FindObjectOfType<Canvas>(); 
                GameObject alert = Instantiate(ptsAlertPrefab, UICanvas.transform);
                alert.transform.position = Camera.main.WorldToScreenPoint(transform.position);
                Destroy(alert, 1f);
            } else
            {
                ShowScore.score += 1;
            }
        }
    }
}

