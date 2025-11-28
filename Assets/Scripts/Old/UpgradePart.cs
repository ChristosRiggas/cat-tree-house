using UnityEngine;

public class UpgradePart : MonoBehaviour
{
    public GameObject unlockedSprite;
    public int cost;
    public string confirmationText;

    private void Awake()
    {
        updateConfirmationText();
    }

    void OnMouseDown()
    {
        if (CurrencyManager.Instance.TrySpend(cost))
        {
            DisplayConfirmationPanel(cost, confirmationText);
            //unlockedSprite.SetActive(true);
            //gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Not enough currency!");
        }
    }

    private void updateConfirmationText()
    {
        confirmationText = "Do you want to upgrade this part for " + cost + " paws?";
    }   

    private void DisplayConfirmationPanel(int cost, string confirmationText)
    {

    }
}
