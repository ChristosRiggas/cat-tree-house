using UnityEngine;

public class CatController : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 8f;      // 移动速度
    [SerializeField] private float minX = -8f;          // 左边界
    [SerializeField] private float maxX = 8f;           // 右边界
    [SerializeField] private Animator animator;          // 猫的动画控制器
    private SpriteRenderer spriteRenderer;

    private void Update()
    {
        // 获取键盘输入 (A/D 或 左右箭头键)
        float horizontalInput = Input.GetAxis("Horizontal");

        if(animator != null)
        {
            // 根据输入设置动画参数
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
        }

        HandleFlipping(horizontalInput);

        // 计算新位置
        Vector3 newPosition = transform.position;
        newPosition.x += horizontalInput * moveSpeed * Time.deltaTime;

        // 限制在屏幕范围内，不让猫跑出边界
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        // 应用新位置
        transform.position = newPosition;
    }

    private void HandleFlipping(float input)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (input > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (input < 0)
        {
            spriteRenderer.flipX = true;
        }
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