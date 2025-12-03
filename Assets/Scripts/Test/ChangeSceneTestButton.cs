using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneTestButton : MonoBehaviour
{
    public Button changeSceneButton;
    public string sceneToLoad;   // <-- assign in Inspector

    private void Start()
    {
        changeSceneButton.onClick.AddListener(OnChangeSceneClicked);
    }

    private void OnChangeSceneClicked()
    {
        if (GameSceneManager.Instance == null)
        {
            Debug.LogError("GameSceneManager singleton not found in scene.");
            return;
        }

        //// Register callback BEFORE loading scene
        //if (sceneToLoad == "Lobby")
        //{
        //    UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        //}

        GameSceneManager.Instance.ChangeScene(sceneToLoad);
    }

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if (scene.name == "Lobby")
    //    {
    //        RecostructCatHouseManager.Instance.ReconstructCatHouse();
    //    }

    //    // Always unsubscribe to avoid duplicate calls
    //    UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    //}
}