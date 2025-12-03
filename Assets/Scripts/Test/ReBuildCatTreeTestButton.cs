using UnityEngine;
using UnityEngine.UI;

public class ReBuildCatTreeTestButton : MonoBehaviour
{
    public Button reBuildButton;

    private void Start()
    {
        reBuildButton.onClick.AddListener(OnRebuiltClicked);
    }

    private void OnRebuiltClicked()
    {
        RecostructCatHouseManager.Instance.ReconstructCatHouse();   
    }
}