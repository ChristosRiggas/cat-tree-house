using TMPro;
using UnityEngine;

public class UpgradePartManager : MonoBehaviour
{
    public static UpgradePartManager Instance;

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
}
