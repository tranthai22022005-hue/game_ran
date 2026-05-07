using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton để script khác gọi:
    // GameManager.Instance.AddScore(...)
    public static GameManager Instance { get; private set; }

    [Header("Game Data")]
    public int score = 0;

    // ===== FIX =====
    // Mỗi food giờ +10 điểm
    // muốn ăn 10 quả mới thắng -> đặt 100
    public int winScore = 100;

    [Header("UI Text")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI resultText;

    [Header("UI Panel")]
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject pausePanel;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1;
        isGameOver = false;
        score = 0;

        UpdateUI();

        // ===== Ẩn panel =====
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (resultText != null)
            resultText.gameObject.SetActive(false);
    }

    // ===== cộng điểm =====
    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;

        Debug.Log("Điểm hiện tại = " + score);

        UpdateUI();

        // ===== check thắng =====
        if (score >= winScore)
        {
            Win();
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        Debug.Log("YOU LOSE");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (resultText != null)
        {
            resultText.text = "YOU LOSE";
            resultText.gameObject.SetActive(true);
        }

        Time.timeScale = 0;
    }

    public void Win()
    {
        if (isGameOver) return;

        isGameOver = true;

        Debug.Log("YOU WIN");

        if (winPanel != null)
            winPanel.SetActive(true);

        if (resultText != null)
        {
            resultText.text = "YOU WIN";
            resultText.gameObject.SetActive(true);
        }

        Time.timeScale = 0;
    }

    public void PauseGame()
    {
        if (isGameOver) return;

        Time.timeScale = 0;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Debug.Log("GAME PAUSED");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Debug.Log("GAME RESUME");
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}