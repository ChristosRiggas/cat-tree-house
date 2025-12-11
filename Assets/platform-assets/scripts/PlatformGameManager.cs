using TMPro;
using Unity.Multiplayer.Center.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;
using System.Collections;

public class PlatformGameManager : MonoBehaviour
{
    public static PlatformGameManager Instance;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    private int coinCount = 0;
    public bool isGameOver = false;
    public TextMeshProUGUI startupHintText;
    public ButterflyController butterflyController;
    public MonoBehaviour playerMovementScript;
    public float flashDuration = 2f;
    public float flashSpeed = 0.2f;  
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (butterflyController != null)
        {
            butterflyController.enabled = false; 
        }
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }
        StartCoroutine(StartGameSequence());
    }
    private IEnumerator StartGameSequence()
    {
        if (startupHintText == null)
        {
            Debug.LogError("No Startup Hint Text !");
            StartGame();
            yield break;
        }

        float startTime = Time.time;
        Color originalColor = startupHintText.color;

        while (Time.time < startTime + flashDuration)
        {
            float alpha = Mathf.Sin(Time.time * (1f / flashSpeed) * Mathf.PI) * 0.5f + 0.5f;

            alpha = Mathf.Clamp(alpha, 0.2f, 1f);

            Color tempColor = originalColor;
            tempColor.a = alpha;
            startupHintText.color = tempColor;

            yield return null; 
        }

        float fadeOutTime = 0.5f;
        float fadeStartTime = Time.time;

        while (Time.time < fadeStartTime + fadeOutTime)
        {
            float progress = (Time.time - fadeStartTime) / fadeOutTime;
            Color tempColor = originalColor;
            tempColor.a = Mathf.Lerp(1f, 0f, progress);
            startupHintText.color = tempColor;

            yield return null;
        }
        startupHintText.gameObject.SetActive(false);

        StartGame();
    }

    private void StartGame()
    {
        if (butterflyController != null)
        {
            butterflyController.enabled = true; 
        }
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
    }
    public void GameOver()
    {
        Debug.Log("GameOver:" + GetCoinCount());
        if (isGameOver) return;

        isGameOver = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = false;

            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            Destroy(player);

        }
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void WinGame()
    {
        int coin = GetCoinCount();
        Debug.Log("win:" + coin);
        if (isGameOver) return;
        if (FixedCoinText.Instance != null)
        {
            FixedCoinText.Instance.StartFadingAnimation(GetCoinCount());
        }
        isGameOver = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            Destroy(player);

        }
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }
    public void CollectCoin(int value = 1)
    {
        coinCount += value;
    }

    public int GetCoinCount()
    {
        return coinCount;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMenu()
    {
       //CurrencyManager.Instance.AddCurrency(GetCoinCount());

       GameSceneManager.Instance.ChangeScene("Lobby"); 
    }
}