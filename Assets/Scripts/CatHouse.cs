using UnityEngine;

public class CatHouse : MonoBehaviour
{
    // Singleton instance
    public static CatHouse Instance { get; private set; }

    //public CatHouseUpgradeData currentUpgrade;
    public Transform currentAttachPoint;
    public Transform modulesParent;

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

    public bool TryApplyUpgrade(CatHouseUpgradeData selected, GameObject lastPlusButton)
    {
        if (selected == null) return false;

        if (!CurrencyManager.Instance.TrySpend(selected.cost))
            return false;

        if (selected.modulePrefab == null)
            return false;

        currentAttachPoint = lastPlusButton.GetComponent<OpenUpgradeButton>().attachPoint;  

        // Spawn upgrade module
        GameObject newModule = Instantiate(selected.modulePrefab, modulesParent);
        newModule.transform.position = currentAttachPoint.position;

        // Update next attach point
        //Transform attach = newModule.transform.Find("AttachPoint");
        //if (attach != null)
        //    currentAttachPoint = attach;

        //currentUpgrade = selected;
        return true;
    }
}
