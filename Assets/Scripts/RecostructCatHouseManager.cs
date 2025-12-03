using System.Collections.Generic;
using UnityEngine;

public class RecostructCatHouseManager : MonoBehaviour
{
    public static RecostructCatHouseManager Instance { get; private set; }

    public Dictionary<int, AppliedUpgradeInfo> appliedUpgrades = new Dictionary<int, AppliedUpgradeInfo>();

    public int appliedUpgradeCounter = 0;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ReconstructCatHouse()
    {
        CatHouse.Instance.DeleteModuleCounters();

        foreach (var kvp in appliedUpgrades)
        {
            AppliedUpgradeInfo info = kvp.Value;
            CatHouse.Instance.RecostructUpgrade(info.upgrade, GameObject.Find(info.buttonPath), info.upgradeIndex);
        }
    }

    public void DebugDictionary()
    {
        //Debug.Log("=== Applied Upgrades Dictionary ===");

        foreach (var kvp in appliedUpgrades)
        {
            int id = kvp.Key;
            AppliedUpgradeInfo info = kvp.Value;

            Debug.Log(
                $"ID: {id} | Upgrade: {info.upgrade.name} | Button Path: {info.buttonPath} | Index: {info.upgradeIndex}"
            );
        }

        //Debug.Log("====================================");
    }

}

[System.Serializable]
public class AppliedUpgradeInfo
{
    public CatHouseUpgradeData upgrade;
    public string buttonPath;
    public int upgradeIndex;

    public AppliedUpgradeInfo(CatHouseUpgradeData upgrade, string buttonPath, int upgradeIndex)
    {
        this.upgrade = upgrade;
        this.buttonPath = buttonPath;
        this.upgradeIndex = upgradeIndex;
    }
}
