using TMPro;
using UnityEngine;

public class ConfirmUpgradePanelUI : MonoBehaviour
{
    public static ConfirmUpgradePanelUI Instance;

    public TMP_Text descriptionText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Show(string confirmationText)
    {
        descriptionText.text = confirmationText;
        gameObject.SetActive(true);
    }
}
