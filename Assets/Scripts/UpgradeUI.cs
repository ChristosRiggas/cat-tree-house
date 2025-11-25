using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance { get; private set; }

    public GameObject panel;
    public Transform listParent;
    public GameObject buttonPrefab;

    private CatHouse currentHouse;
    private GameObject lastPlusButton;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // Optional: persist across scenes
        // DontDestroyOnLoad(gameObject);
    }

    public void Open(CatHouse house, GameObject lastSelectedPlusButton)
    {
        lastPlusButton = lastSelectedPlusButton;
        currentHouse = house;
        panel.SetActive(true);
        RefreshButtons();
    }

    public void Close()
    {
        panel.SetActive(false);
        foreach (Transform child in listParent)
            Destroy(child.gameObject);
    }

    private void RefreshButtons()
    {
        foreach (Transform child in listParent)
            Destroy(child.gameObject);

        foreach (var upgrade in currentHouse.currentUpgrade.nextUpgrades)
        {
            var btn = Instantiate(buttonPrefab, listParent);
            btn.GetComponent<UpgradeOptionButton>()
               .Setup(upgrade, this);
        }
    }

    public void OnUpgradeChosen(CatHouseUpgradeData upgrade)
    {
        if (currentHouse.TryApplyUpgrade(upgrade))
        {
            Close();
            lastPlusButton.SetActive(false);
        }
    }
}
