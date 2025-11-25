using UnityEngine;
using UnityEngine.UI;

public class UpgradeOptionButton : MonoBehaviour
{
    [Header("UI")]
    public Image icon;
    public TMPro.TextMeshProUGUI label;
    public TMPro.TextMeshProUGUI cost;

    private CatHouseUpgradeData upgrade;
    private UpgradeUI ui;

    public void Setup(CatHouseUpgradeData data, UpgradeUI uiController)
    {
        upgrade = data;
        ui = uiController;

        icon.sprite = data.icon;
        label.text = data.upgradeName;
        cost.text = data.cost.ToString();

        GetComponent<Button>().onClick.AddListener(OnPressed);
    }

    private void OnPressed()
    {
        ui.OnUpgradeChosen(upgrade);
    }
}
