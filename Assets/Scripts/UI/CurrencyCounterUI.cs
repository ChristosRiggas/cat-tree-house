using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CurrencyCounterUI : MonoBehaviour
{
    public static CurrencyCounterUI Instance;

    public TMP_Text currencyText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void UpdateCurrencyText(int currencyAmount)
    {
        if (currencyText != null)
        {
            currencyText.text = /*"Paws: " + */currencyAmount.ToString();
        }
    }
}
