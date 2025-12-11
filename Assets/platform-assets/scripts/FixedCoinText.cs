using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class FixedCoinText : MonoBehaviour
{
    public static FixedCoinText Instance;

    public float moveDistance = 20f; 
    public float fadeDuration = 1.5f;

    private TextMeshProUGUI textComponent;
    private RectTransform rectTransform;
    private Color startColor;
    private Vector2 initialPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        textComponent = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();

        if (textComponent != null)
        {
            startColor = textComponent.color;
        }

        initialPosition = rectTransform.anchoredPosition;

        gameObject.SetActive(false);
    }

    public void StartFadingAnimation(int coinAmount)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ShowFadingCoinText(coinAmount));

        CurrencyManager.Instance.AddCurrency(coinAmount * 3);
    }

    private IEnumerator ShowFadingCoinText(int coinAmount)
    {
        gameObject.SetActive(true);
        textComponent.text = $"+ {coinAmount} ¡Á 3";

        rectTransform.anchoredPosition = initialPosition;
        textComponent.color = startColor;
        
        Vector2 endPosition = initialPosition + Vector2.up * moveDistance;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float progress = elapsedTime / fadeDuration;

            rectTransform.anchoredPosition = Vector2.Lerp(initialPosition, endPosition, progress);

            Color currentColor = textComponent.color;
            currentColor.a = Mathf.Lerp(1f, 0f, progress);
            textComponent.color = currentColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false); 
    }
}