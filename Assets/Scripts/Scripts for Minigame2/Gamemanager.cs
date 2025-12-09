using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;  // 添加 TextMeshPro 支持

public class GameManager : MonoBehaviour
{
    [Header("UI引用 - Legacy Text")]
    [SerializeField] private Text scoreText;            // 分数显示文本（Legacy）
    [SerializeField] private Text timeText;             // 倒计时显示文本（Legacy）
    [SerializeField] private Text finalScoreText;       // 最终分数显示（Legacy）

    [Header("UI引用 - TextMeshPro（如果使用TMP就用这个）")]
    [SerializeField] private TextMeshProUGUI scoreTextTMP;      // 分数显示（TMP）
    [SerializeField] private TextMeshProUGUI timeTextTMP;        // 倒计时显示（TMP）
    [SerializeField] private TextMeshProUGUI finalScoreTextTMP;  // 最终分数（TMP）

    [Header("其他UI")]
    [SerializeField] private GameObject gameOverPanel;  // 游戏结束面板

    [Header("游戏设置")]
    [SerializeField] private float gameTime = 60f;      // 游戏时间（秒）
    [SerializeField] private string nextSceneName = "MainMenu";  // 游戏结束后跳转的场景名

    [Header("其他引用")]
    [SerializeField] private ItemSpawner itemSpawner;   // 物品生成器引用

    private int currentScore = 0;
    private float timeRemaining;
    private bool isGameOver = false;

    private void Start()
    {
        // 初始化
        timeRemaining = gameTime;
        currentScore = 0;
        UpdateScoreUI();
        UpdateTimeUI();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // 开始生成物品
        if (itemSpawner != null)
        {
            itemSpawner.StartSpawning();
        }
    }

    private void Update()
    {
        if (isGameOver) return;

        // 倒计时
        timeRemaining -= Time.deltaTime;
        UpdateTimeUI();

        // 时间到，游戏结束
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            GameOver();
        }
    }

    // 增加分数（抓到鱼时调用）
    public void AddScore(int points)
    {
        if (isGameOver) return;

        currentScore += points;
        UpdateScoreUI();

        // 可以在这里添加音效和视觉反馈
        Debug.Log("Score: " + currentScore);
    }

    // 游戏结束（抓到洋葱或时间到）
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        // 停止生成物品
        if (itemSpawner != null)
        {
            itemSpawner.StopSpawning();
        }

        // 显示游戏结束界面
        ShowGameOverPanel();

        Debug.Log("Game Ends, Final Score: " + currentScore);
    }

    // 显示游戏结束面板
    private void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        string finalScoreString = "Final Score: " + currentScore;

        if (finalScoreText != null)
        {
            finalScoreText.text = finalScoreString;
        }

        if (finalScoreTextTMP != null)
        {
            finalScoreTextTMP.text = finalScoreString;
        }
    }

    // 更新分数UI
    private void UpdateScoreUI()
    {
        string scoreString = "Score: " + currentScore;

        if (scoreText != null)
        {
            scoreText.text = scoreString;
        }

        if (scoreTextTMP != null)
        {
            scoreTextTMP.text = scoreString;
        }
    }

    // 更新时间UI
    private void UpdateTimeUI()
    {
        int seconds = Mathf.CeilToInt(timeRemaining);
        string timeString = "Time: " + seconds + "s";

        if (timeText != null)
        {
            timeText.text = timeString;
        }

        if (timeTextTMP != null)
        {
            timeTextTMP.text = timeString;
        }
    }

    // 结束按钮调用 - 跳转到其他场景
    public void OnEndButtonClick()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    // 重新开始按钮调用 - 重新加载当前场景
    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}