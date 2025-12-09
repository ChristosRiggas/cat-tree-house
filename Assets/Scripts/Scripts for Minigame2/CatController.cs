using UnityEngine;

public class CatController : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 8f;      // 移动速度
    [SerializeField] private float minX = -8f;          // 左边界
    [SerializeField] private float maxX = 8f;           // 右边界

    private void Update()
    {
        // 获取键盘输入 (A/D 或 左右箭头键)
        float horizontalInput = Input.GetAxis("Horizontal");

        // 计算新位置
        Vector3 newPosition = transform.position;
        newPosition.x += horizontalInput * moveSpeed * Time.deltaTime;

        // 限制在屏幕范围内，不让猫跑出边界
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        // 应用新位置
        transform.position = newPosition;
    }

    // 碰撞检测 - 当猫接触到掉落物品时触发
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰到的是不是掉落物品
        FallingItem item = collision.GetComponent<FallingItem>();
        if (item != null)
        {
            // 调用物品的收集方法
            item.Collect();
        }
    }
}