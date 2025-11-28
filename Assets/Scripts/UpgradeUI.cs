using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance { get; private set; }

    public GameObject panel;
    public Transform listParent;
    public GameObject buttonPrefab;

    private CatHouse currentHouse;
    private GameObject lastPlusButton;

    public CatHouseUpgradeData[] currentNextUpgrades;

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
        UpdateCurrentUpgradesBasedOnButton(lastPlusButton);
        RefreshButtons();
    }

    private void UpdateCurrentUpgradesBasedOnButton(GameObject plusButton)
    {
        var openUpgradeButton = plusButton.GetComponent<OpenUpgradeButton>();
        if (openUpgradeButton != null && openUpgradeButton.nextUpgrades != null)
        {
            currentNextUpgrades = openUpgradeButton.nextUpgrades;
        }
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

        int index = 0;

        foreach (var upgrade in currentNextUpgrades)
        {
            var btn = Instantiate(buttonPrefab, listParent);
            btn.GetComponent<UpgradeOptionButton>()
               .Setup(upgrade, this, index);

            index++;    
        }
    }

    //private void RefreshButtons()
    //{
    //    foreach (Transform child in listParent)
    //        Destroy(child.gameObject);

    //    foreach (var upgrade in currentHouse.currentUpgrade.nextUpgrades)
    //    {
    //        var btn = Instantiate(buttonPrefab, listParent);
    //        btn.GetComponent<UpgradeOptionButton>()
    //           .Setup(upgrade, this);
    //    }
    //}

    public void OnUpgradeChosen(CatHouseUpgradeData upgrade, int upgradeIndex)
    {
        if (currentHouse.TryApplyUpgrade(upgrade, lastPlusButton, upgradeIndex))
        {
            Close();
            lastPlusButton.SetActive(false);
        }
    }
}
