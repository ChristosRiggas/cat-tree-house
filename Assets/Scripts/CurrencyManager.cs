using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

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
