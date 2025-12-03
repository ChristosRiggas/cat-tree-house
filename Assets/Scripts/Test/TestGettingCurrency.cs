using UnityEngine;
using UnityEngine.UI;

public class TestGettingCurrency : MonoBehaviour
{
    public Button currencyButton;

    private void Start()
    {
        currencyButton.onClick.AddListener(OnClickedCurrency);
    }

    private void OnClickedCurrency()
    {
        CurrencyManager.Instance.AddCurrency(10);
        Debug.Log("Added 10 currency. Current amount: " + CurrencyManager.Instance.currencyAmount);
    }
}