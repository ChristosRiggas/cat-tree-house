using UnityEngine;

public class FallingItem : MonoBehaviour
{
    [Header("物品设置")]
    [SerializeField] private float fallSpeed = 3f;          // 掉落速度
    [SerializeField] private bool isCorrectItem = true;     // true=老鼠(正确), false=洋葱(错误)
    [SerializeField] private int score = 10;                // 抓到老鼠的得分

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
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            if (isCorrectItem)
            {
                // 抓到老鼠 - 加分
                gameManager.AddScore(score);
                Debug.Log("抓到老鼠! +{score}分");
            }
            else
            {
                // 抓到洋葱 - 游戏结束
                gameManager.GameOver();
                Debug.Log("抓到洋葱! 游戏结束");
            }
        }

        // 销毁这个物品
        Destroy(gameObject);
    }
}