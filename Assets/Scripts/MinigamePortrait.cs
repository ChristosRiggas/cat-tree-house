using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MinigamePortrait : MonoBehaviour
{
    [Header("Cooldown")]
    public int cooldownTime = 5;

    private bool available = false;
    private bool isOnCooldown = false;

    private Button button;

    public string sceneToLoad;
    public int requiredHeight = 10;

    void Awake()
    {
        button = GetComponentInChildren<Button>();
        button.interactable = false;
        button.onClick.AddListener(OnButtonPressed);
    }

    private void Start()
    {
        StartCooldown();
    }

    private void Update()
    {
        CheckIfTheMinigameIsAvailable();
    }

    public void CheckIfTheMinigameIsAvailable()
    {
        if(requiredHeight <= CurrencyManager.Instance.treeHeight)
        {
            available = true;
        }
        else
        {
            available = false;
        }

        UpdateButtonState();
    }

    // ---------- BUTTON ----------

    private void OnButtonPressed()
    {
        if (!button.interactable) return;

        GameSceneManager.Instance.ChangeScene(sceneToLoad);
        StartCooldown();
    }

    // ---------- COOLDOWN ----------

    private void StartCooldown()
    {
        isOnCooldown = true;
        UpdateButtonState();
        StartCoroutine(CooldownTimer());
    }

    IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(cooldownTime);

        isOnCooldown = false;
        UpdateButtonState();
    }

    // ---------- STATE ----------

    private void UpdateButtonState()
    {
        button.interactable = available && !isOnCooldown;
    }
}
