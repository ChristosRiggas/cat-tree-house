using TMPro;
using UnityEngine;

public class ModeUI : MonoBehaviour
{
    public static ModeUI Instance;

    public TMP_Text modeText;

    public GameObject upgradeImage;
    public GameObject moveImaget;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void UpdateModeText(string text)
    {
        //if (modeText != null)
        //{
        //    modeText.text = text;
        //}
        if(text == "Upgrade Mode")
        {
            upgradeImage.SetActive(true);
            moveImaget.SetActive(false);
        }
        else
        {
            upgradeImage.SetActive(false);
            moveImaget.SetActive(true);
        }
    }
}
