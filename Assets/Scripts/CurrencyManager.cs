using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    // --- Timer Variables ---
    private float currencyInterval = 3.0f; // Time in seconds between passive currency awards
    private float timer = 0f; 

    public int currencyAmount = 200;

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

        switch (zone)
        {
            case SnapZoneType.OpenHouse:
                AddCurrency(5);
                break;
            case SnapZoneType.ClosedHouse:
                AddCurrency(3);
                break;
            case SnapZoneType.SideBed:
                AddCurrency(2);
                break;
            case SnapZoneType.SidePlatform:
                AddCurrency(1);
                break;
            case SnapZoneType.CryPlace:
                AddCurrency(0);
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
}
