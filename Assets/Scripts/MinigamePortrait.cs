//using System.Collections;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class MinigamePortrait : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
//{
//    [Header("Cooldown")]
//    public int cooldownTime = 5;

//    private bool available = false;
//    private bool isOnCooldown = false;

//    private Button button;

//    public string sceneToLoad;
//    public int requiredHeight = 10;

//    [SerializeField] private GameObject descriptionPanel;

//    void Awake()
//    {
//        button = GetComponentInChildren<Button>();
//        button.interactable = false;
//        button.onClick.AddListener(OnButtonPressed);
//    }

//    private void Start()
//    {
//        if (descriptionPanel != null)
//            descriptionPanel.SetActive(false);

//        StartCooldown();
//    }


//    public void OnPointerEnter(PointerEventData eventData)
//    {
//        if (available)
//        {
//            if (descriptionPanel != null)
//                descriptionPanel.SetActive(true);
//        }
//    }

//    public void OnPointerExit(PointerEventData eventData)
//    {
//        if (available)
//        {
//            if (descriptionPanel != null)
//                descriptionPanel.SetActive(false);
//        }
//    }

//    private void Update()
//    {
//        CheckIfTheMinigameIsAvailable();

//        if (UpgradePartManager.Instance.upgradeMode || isOnCooldown)
//        {
//            descriptionPanel.SetActive(false);
//        }
//    }

//    public void CheckIfTheMinigameIsAvailable()
//    {
//        if(UpgradePartManager.Instance != null && UpgradePartManager.Instance.upgradeMode)
//        {
//            available = false;
//            UpdateButtonState();
//            GetComponentInParent<Canvas>().sortingOrder = -9;
//            return;
//        }

//        if (requiredHeight <= CurrencyManager.Instance.treeHeight)
//        {
//            available = true;
//        }
//        else
//        {
//            available = false;
//        }

//        Canvas canvas = GetComponentInParent<Canvas>();

//        if (available && !UpgradePartManager.Instance.upgradeMode)
//            canvas.sortingOrder = -9; // 100 before but looks weird
//        else
//            canvas.sortingOrder = -9;

//        UpdateButtonState();
//    }

//    // ---------- BUTTON ----------

//    private void OnButtonPressed()
//    {
//        if (!button.interactable) return;

//        GameSceneManager.Instance.ChangeScene(sceneToLoad);
//        StartCooldown();
//    }

//    // ---------- COOLDOWN ----------

//    private void StartCooldown()
//    {
//        isOnCooldown = true;
//        UpdateButtonState();
//        StartCoroutine(CooldownTimer());
//    }

//    IEnumerator CooldownTimer()
//    {
//        yield return new WaitForSeconds(cooldownTime);

//        isOnCooldown = false;
//        UpdateButtonState();
//    }

//    // ---------- STATE ----------

//    private void UpdateButtonState()
//    {
//        button.interactable = available && !isOnCooldown;
//    }
//}

using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinigamePortrait : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Minigame")]
    public string sceneToLoad;
    public int requiredHeight = 10;

    [Header("Cooldown")]
    public int cooldownTime = 5;

    [Header("UI")]
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private TMP_Text cooldownText;

    private Button button;
    private bool available = false;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
        button.interactable = false;
        button.onClick.AddListener(OnButtonPressed);
    }

    private void Start()
    {
        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);

        UpdateButtonState();
    }

    private void Update()
    {
        CheckAvailability();

        // Hide description if not usable
        if (!available || IsOnCooldown() || UpgradePartManager.Instance.upgradeMode)
        {
            if (descriptionPanel != null)
                descriptionPanel.SetActive(false);
        }

        float remaining = MinigamePortraitsTimeManager.Instance
                    .GetRemainingTime(sceneToLoad);

        if (IsOnCooldown())
        {
            cooldownText.text = $"{remaining:F0}";
        }
        else if(available)
        {
            cooldownText.text = "Ready";
        }
        else
        {
            cooldownText.text = "Locked";
        }
    }
    // ---------- HOVER ----------

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (available && !IsOnCooldown())
        {
            if (descriptionPanel != null)
                descriptionPanel.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);
    }

    // ---------- AVAILABILITY ----------

    private void CheckAvailability()
    {
        if (UpgradePartManager.Instance != null && UpgradePartManager.Instance.upgradeMode)
        {
            available = false;
            UpdateButtonState();
            return;
        }

        available = CurrencyManager.Instance.treeHeight >= requiredHeight;
        UpdateButtonState();
    }

    // ---------- BUTTON ----------

    private void OnButtonPressed()
    {
        if (!button.interactable) return;

        MinigamePortraitsTimeManager.Instance.StartCooldown(
            sceneToLoad,
            cooldownTime
        );

        GameSceneManager.Instance.ChangeScene(sceneToLoad);
    }

    // ---------- STATE ----------

    private bool IsOnCooldown()
    {
        return MinigamePortraitsTimeManager.Instance
            .IsOnCooldown(sceneToLoad);
    }

    private void UpdateButtonState()
    {
        button.interactable = available && !IsOnCooldown();
    }
}

