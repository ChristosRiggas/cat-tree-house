using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    // --- Timer Variables ---
    private float currencyInterval = 3.0f; // Time in seconds between passive currency awards
    private float timer = 0f; 

    public int currencyAmount = 200;

    public int treeHeight = 0;
    private int treeHeightMultiplier = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CurrencyCounterUI.Instance.UpdateCurrencyText(currencyAmount);
    }

    private void Update()
    {
        if(MoveTheCat.Instance == null || MoveTheCat.Instance.isCarried)
            return;

        timer += Time.deltaTime;

        if (timer >= currencyInterval)
        {
            timer = 0f;
            AwardPassiveCurrency();
        }
    }

    private void AwardPassiveCurrency()
    {
        if(MoveTheCat.Instance == null || MoveTheCat.Instance.currentZoneData == null)
            return;
        SnapZoneType zone = MoveTheCat.Instance.currentZoneData.zoneType;

        Debug.Log("Awarding passive currency for zone: " + zone.ToString());

        //treeHeightMultiplier = Mathf.Max(1, Mathf.CeilToInt(treeHeight / 10f));

        float currentHeightInRestZone = MoveTheCat.Instance.currentZoneData.gameObject.GetComponentInParent<Transform>().position.y;

        treeHeightMultiplier = Mathf.Max(1, Mathf.CeilToInt((currentHeightInRestZone) / 5f));

        switch (zone)
        {
            case SnapZoneType.OpenHouse:
                AddCurrency(5 * treeHeightMultiplier);
                break;
            case SnapZoneType.ClosedHouse:
                AddCurrency(4 * treeHeightMultiplier);
                break;
            case SnapZoneType.SideBed:
                AddCurrency(2 * treeHeightMultiplier);
                break;
            case SnapZoneType.SidePlatform:
                AddCurrency(1 * treeHeightMultiplier);
                break;
            case SnapZoneType.CryPlace:
                AddCurrency(1);
                break;
        }
    }

    public bool TrySpend(int amount)
    {
        if (currencyAmount >= amount)
        {
            currencyAmount -= amount;
            CurrencyCounterUI.Instance.UpdateCurrencyText(currencyAmount);
            return true;
        }

        return false;
    }

    public void AddCurrency(int amount)
    {
        currencyAmount += amount;
        CurrencyCounterUI.Instance.UpdateCurrencyText(currencyAmount);
    }

    public void SetTreeHeight(int height)
    {
        treeHeight = height;
        //TreeHeightUI.Instance.UpdateTreeHeightText(treeHeight);
    }
}
