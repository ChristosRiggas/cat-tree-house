using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance { get; private set; }

    public GameObject panel;
    public Transform listParent;
    public GameObject buttonPrefab;

    private CatHouse currentHouse;
    public GameObject lastPlusButton;

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

            string path = GetHierarchyPath(lastPlusButton.transform);
            // Create upgrade info object
            AppliedUpgradeInfo info = new AppliedUpgradeInfo(upgrade, path, upgradeIndex);

            // Add to dictionary with incrementing ID
            RecostructCatHouseManager.Instance.appliedUpgradeCounter++;
            RecostructCatHouseManager.Instance.appliedUpgrades.Add(RecostructCatHouseManager.Instance.appliedUpgradeCounter, info);

            RecostructCatHouseManager.Instance.DebugDictionary();

            //Debug.Log($"Upgrade added with ID: {RecostructCatHouseManager.Instance.appliedUpgradeCounter}, path: {path}");
        }
    }

    public static string GetHierarchyPath(Transform t)
    {
        string path = t.name;
        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
}
