using System.Collections.Generic;
using UnityEngine;

public class CatHouse : MonoBehaviour
{
    // Singleton instance
    public static CatHouse Instance { get; private set; }

    //public CatHouseUpgradeData currentUpgrade;
    public Transform currentAttachPoint;
    public Transform modulesParent;

    public int currentShortingLayerValue = 0;

    private Dictionary<string, int> moduleCounters = new();

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

    public bool RecostructUpgrade(CatHouseUpgradeData selected, GameObject lastPlusButton, int upgradeIndex)
    {
        if (selected == null) return false;

        if (selected.modulePrefab == null)
            return false;

        // Update attach point to the one from the last plus button
        currentAttachPoint = lastPlusButton.GetComponent<OpenUpgradeButton>().attachPoint[upgradeIndex];

        Destroy(lastPlusButton);

        // Spawn upgrade module
        GameObject newModule = Instantiate(selected.modulePrefab, modulesParent);
        newModule.transform.position = currentAttachPoint.position;

        newModule.GetComponent<SpriteRenderer>().sortingOrder = currentShortingLayerValue;
        currentShortingLayerValue++;

        RenameModuleGO(newModule);
        return true;
    }

    public bool TryApplyUpgrade(CatHouseUpgradeData selected, GameObject lastPlusButton, int upgradeIndex)
    {
        if (selected == null) return false;

        if (!CurrencyManager.Instance.TrySpend(selected.cost))
            return false;

        if (selected.modulePrefab == null)
            return false;

        //int index = FindIndexOfSelectedUpgrade(selected, lastPlusButton);

        //if(index == -1)
        //    return false;

        // Update attach point to the one from the last plus button
        currentAttachPoint = lastPlusButton.GetComponent<OpenUpgradeButton>().attachPoint[upgradeIndex];

        Destroy(lastPlusButton);

        // Spawn upgrade module
        GameObject newModule = Instantiate(selected.modulePrefab, modulesParent);
        newModule.transform.position = currentAttachPoint.position;

        newModule.GetComponent<SpriteRenderer>().sortingOrder = currentShortingLayerValue;
        currentShortingLayerValue++;

        RenameModuleGO(newModule);

        // Update next attach point
        //Transform attach = newModule.transform.Find("AttachPoint");
        //if (attach != null)
        //    currentAttachPoint = attach;

        // Get the Y position of the new module relative to the tree parent
        float moduleY = newModule.transform.localPosition.y;
        if(moduleY < 0)
            moduleY = 0;

        // Add the module's extra height (if any)
        float totalHeight = moduleY + selected.heightIncrease;
        Debug.Log($"Module Y: {moduleY}, Height Increase: {selected.heightIncrease}, Total Height: {totalHeight}");

        //// Convert to integer
        int newHeight = Mathf.CeilToInt(totalHeight);

        // Update the manager if this module makes the tree taller
        if (newHeight > CurrencyManager.Instance.treeHeight)
        {
            CurrencyManager.Instance.SetTreeHeight(newHeight);
        }

        return true;
    }

    private void RenameModuleGO(GameObject module)
    {
        string baseName = module.name.Split('(')[0].Trim(); // Get the base name without any (Clone) or (1) suffix
        if (!moduleCounters.ContainsKey(baseName))
        {
            moduleCounters[baseName] = 1;
        }
        else
        {
            moduleCounters[baseName]++;
        }
        module.name = $"{baseName} ({moduleCounters[baseName]})";
    }

    public void DeleteModuleCounters()
    {
        moduleCounters.Clear();
    }

    private int FindIndexOfSelectedUpgrade(CatHouseUpgradeData selected, GameObject lastPlusButton)
    {
        var openUpgradeButton = lastPlusButton.GetComponent<OpenUpgradeButton>();

        if (openUpgradeButton == null) return -1;

        for (int i = 0; i < openUpgradeButton.nextUpgrades.Length; i++)
        {
            if (openUpgradeButton.nextUpgrades[i] == selected)
            {
                // Here you can store the index if needed
                Debug.Log($"Selected upgrade index: {i}");
                return i;
            }
        }

        return -1; // Not found
    }
}
