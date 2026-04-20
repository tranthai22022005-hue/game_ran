using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public int winScore = 10;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI resultText;

    private bool isGameOver = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
        resultText.gameObject.SetActive(false);
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        UpdateUI();

        if (score >= winScore)
        {
            Win();
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        resultText.text = "YOU LOSE";
        resultText.gameObject.SetActive(true);

        Time.timeScale = 0; // 🔥 dừng game
    }

    public void Win()
    {
        if (isGameOver) return;

        isGameOver = true;

        resultText.text = "YOU WIN";
        resultText.gameObject.SetActive(true);

        Time.timeScale = 0; // 🔥 dừng game
    }
}