using UnityEngine;

[CreateAssetMenu(menuName = "CatHouse/Upgrade Option")]
public class CatHouseUpgradeData : ScriptableObject
{
    public string upgradeName;
    public Sprite icon;
    public int cost;
    public string description;
    public int heightIncrease;

    public GameObject modulePrefab;       // The prefab to spawn
    //public CatHouseUpgradeData[] nextUpgrades;
}