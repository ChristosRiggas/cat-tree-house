using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Lobby")
        {
            RecostructCatHouseManager.Instance.ReconstructCatHouse();
            CurrencyCounterUI.Instance.UpdateCurrencyText(CurrencyManager.Instance.currencyAmount);
            //TreeHeightUI.Instance.UpdateTreeHeightText(CurrencyManager.Instance.treeHeight);
        }

        // Always unsubscribe to avoid duplicate calls
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}