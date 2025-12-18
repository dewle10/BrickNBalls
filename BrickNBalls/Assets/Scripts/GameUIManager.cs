using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI BallsText;
    public GameObject GameOverPanel;
    public TextMeshProUGUI FinalScoreText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UpdateScore(int score)
    {
        ScoreText.text = $"Score: {score}";
    }

    public void UpdateBalls(int Balls)
    {
        BallsText.text = $"Balls: {Balls}";
    }
    public void ShowGameOverPanel(int finalScore)
    {
        GameOverPanel.SetActive(true);
        FinalScoreText.text = $"Final Score: {finalScore}";
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}