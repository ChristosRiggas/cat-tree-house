using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("预制体引用")]
    [SerializeField] private GameObject fishPrefab;         // 鱼的预制体
    [SerializeField] private GameObject onionPrefab;        // 洋葱的预制体

    [Header("生成设置")]
    [SerializeField] private float spawnInterval = 1.5f;    // 初始生成间隔（秒）
    [SerializeField] private float minX = -7f;              // 生成区域左边界
    [SerializeField] private float maxX = 7f;               // 生成区域右边界
    [SerializeField] private float spawnY = 6f;             // 生成高度

    [Header("难度递增设置")]
    [SerializeField] private bool enableDifficulty = true;          // 是否启用难度递增
    [SerializeField] private float minSpawnInterval = 0.5f;         // 最小生成间隔（最快）
    [SerializeField] private float difficultyIncreaseRate = 0.05f;  // 每秒减少的间隔时间

    [Header("物品比例")]
    [SerializeField][Range(0f, 1f)] private float fishProbability = 0.8f;  // 初始生成鱼的概率（80%）
    [SerializeField] private bool enableOnionIncrease = true;               // 是否启用洋葱比例递增
    [SerializeField][Range(0f, 1f)] private float minFishProbability = 0.4f;  // 最低鱼概率（最多60%洋葱）
    [SerializeField] private float onionIncreaseRate = 0.01f;               // 每秒减少的鱼概率（增加洋葱）

    private float spawnTimer = 0f;
    private bool isSpawning = true;
    private float currentSpawnInterval;     // 当前的生成间隔
    private float gameTime = 0f;            // 游戏运行时间
    private float currentFishProbability;   // 当前的鱼概率

    private void Update()
    {
        if (!isSpawning) return;

        // 更新游戏时间
        gameTime += Time.deltaTime;

        // 难度递增：随着时间推移，生成间隔越来越短
        if (enableDifficulty)
        {
            currentSpawnInterval = spawnInterval - (gameTime * difficultyIncreaseRate);
            currentSpawnInterval = Mathf.Max(currentSpawnInterval, minSpawnInterval);
        }
        else
        {
            currentSpawnInterval = spawnInterval;
        }

        // 洋葱比例递增：随着时间推移，洋葱越来越多
        if (enableOnionIncrease)
        {
            currentFishProbability = fishProbability - (gameTime * onionIncreaseRate);
            currentFishProbability = Mathf.Max(currentFishProbability, minFishProbability);
        }
        else
        {
            currentFishProbability = fishProbability;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= currentSpawnInterval)
        {
            SpawnItem();
            spawnTimer = 0f;
        }
    }

    private void SpawnItem()
    {
        // 随机生成位置
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);

        // 根据当前概率决定生成鱼还是洋葱
        GameObject itemToSpawn;
        if (Random.value < currentFishProbability)
        {
            itemToSpawn = fishPrefab;  // 生成鱼
        }
        else
        {
            itemToSpawn = onionPrefab;  // 生成洋葱
        }

        // 实例化物品
        if (itemToSpawn != null)
        {
            Instantiate(itemToSpawn, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("物品预制体未设置！请在 Inspector 中拖入鱼和洋葱的 Prefab");
        }
    }

    // 停止生成（游戏结束时调用）
    public void StopSpawning()
    {
        isSpawning = false;
    }

    // 开始生成（游戏开始时调用）
    public void StartSpawning()
    {
        isSpawning = true;
        spawnTimer = 0f;
        gameTime = 0f;
        currentSpawnInterval = spawnInterval;
        currentFishProbability = fishProbability;
    }
}