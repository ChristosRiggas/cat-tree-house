using UnityEngine;

public class FallingItem : MonoBehaviour
{
    [Header("物品设置")]
    [SerializeField] private float fallSpeed = 3f;          // 掉落速度
    [SerializeField] private bool isCorrectItem = true;     // true=鱼(正确), false=洋葱(错误)
    [SerializeField] private int score = 10;                // 抓到鱼的得分

    [Header("边界设置")]
    [SerializeField] private float destroyY = -6f;          // 掉出屏幕底部的Y坐标

    private void Update()
    {
        // 物品向下掉落
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // 如果掉出屏幕底部，销毁物品
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }

    // 被猫抓到时调用
    public void Collect()
    {
        // 找到游戏管理器
        GameManager gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            if (isCorrectItem)
            {
                // 抓到鱼 - 加分
                gameManager.AddScore(score);
                Debug.Log("Caught fish! +" + score + " points");
            }
            else
            {
                // 抓到洋葱 - 游戏结束
                gameManager.GameOver();
                Debug.Log("Caught onion! Game Over");
            }
        }

        // 销毁这个物品
        Destroy(gameObject);
    }
}