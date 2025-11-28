using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradePartManager : MonoBehaviour
{
    public static UpgradePartManager Instance;

    private GameObject[] upgradeButtons;
    private GameObject[] upgradeParts;
    public GameObject firstUpgradeButton;

    public bool upgradeMode = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (upgradeMode)
        {
            ModeUI.Instance.UpdateModeText("Upgrade Mode");
            firstUpgradeButton.SetActive(true);
        }
        else
        {
            ModeUI.Instance.UpdateModeText("Move Mode");
            firstUpgradeButton.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            upgradeMode = !upgradeMode;

            if(upgradeMode)
            {
                ModeUI.Instance.UpdateModeText("Upgrade Mode");
                MoveTheCat.Instance.isCarried = false;
                MoveTheCat.Instance.CancelPlacement();
                if(firstUpgradeButton != null)
                    firstUpgradeButton.SetActive(true);
            }
            else
            {
                ModeUI.Instance.UpdateModeText("Move Mode");
                if(firstUpgradeButton != null)
                    firstUpgradeButton.SetActive(false);
            }
        }
    }

    //public void DisableAllUpgradeButtonsAndColliders()
    //{
    //    if (upgradeButtons == null && upgradeParts == null)
    //    {
    //        upgradeButtons = GameObject.FindGameObjectsWithTag("UpgradeButton");
    //        upgradeParts = FindObjectsInLayer(LayerMask.NameToLayer("Part"));
    //    }

    //    foreach (var button in upgradeButtons)
    //    {
    //        button.SetActive(false);
    //    }

    //    foreach (var part in upgradeParts)
    //    {
    //        part.GetComponent<Collider2D>().enabled = false;
    //    }   
    //}

    //public void EnableAllUpgradeButtonsAndColliders()
    //{
    //    if (upgradeButtons == null)
    //    {
    //        upgradeButtons = GameObject.FindGameObjectsWithTag("UpgradeButton");
    //    }

    //    foreach (var button in upgradeButtons)
    //    {
    //        button.SetActive(true);
    //    }

    //    foreach (var part in upgradeParts)
    //    {
    //        part.GetComponent<Collider2D>().enabled = true;
    //    }
    //}

    //public static GameObject[] FindObjectsInLayer(int layer)
    //{
    //    GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
    //    List<GameObject> list = new List<GameObject>();

    //    foreach (var obj in allObjects)
    //    {
    //        if (obj.layer == layer)
    //            list.Add(obj);
    //    }

    //    return list.ToArray();
    //}
}
