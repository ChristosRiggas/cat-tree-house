using UnityEngine;

public class OpenUpgradeButton : MonoBehaviour
{
    public UpgradeUI ui;
    public CatHouse catHouse;
    public Transform[] attachPoint;
    public CatHouseUpgradeData[] nextUpgrades;

    public void Awake()
    {
        if (ui == null)
            ui = UpgradeUI.Instance;

        if (catHouse == null)
            catHouse = CatHouse.Instance;
    }

    public void Open()
    {
        ui.Open(catHouse, this.gameObject);
    }
}
