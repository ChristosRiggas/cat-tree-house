using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformGameManager : MonoBehaviour
{
    public static PlatformGameManager Instance;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    private int coinCount = 0;
    public bool isGameOver = false;

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
        Debug.Log("win:" + GetCoinCount());
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
        
       // SceneManager.LoadScene(menuSceneName); 
    }
}