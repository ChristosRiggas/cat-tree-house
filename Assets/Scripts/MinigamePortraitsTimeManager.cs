using System.Collections.Generic;
using UnityEngine;

public class MinigamePortraitsTimeManager : MonoBehaviour
{
    public static MinigamePortraitsTimeManager Instance;

    // Key = minigame ID (scene name)
    // Value = time when cooldown ends
    private Dictionary<string, float> cooldownEndTimes = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        foreach (var kvp in cooldownEndTimes)
        {
            float remaining = Mathf.Max(0, kvp.Value - Time.time);
            //Debug.Log($"Minigame: {kvp.Key} | Remaining cooldown: {remaining:F2}s");
        }
    }

    // Start cooldown for a minigame
    public void StartCooldown(string minigameID, float cooldownDuration)
    {
        cooldownEndTimes[minigameID] = Time.time + cooldownDuration;
    }

    // Check if a minigame is on cooldown
    public bool IsOnCooldown(string minigameID)
    {
        if (!cooldownEndTimes.ContainsKey(minigameID))
            return false;

        return Time.time < cooldownEndTimes[minigameID];
    }

    // Optional: remaining cooldown time
    public float GetRemainingTime(string minigameID)
    {
        if (!cooldownEndTimes.ContainsKey(minigameID))
            return 0f;

        return Mathf.Max(0f, cooldownEndTimes[minigameID] - Time.time);
    }
}
