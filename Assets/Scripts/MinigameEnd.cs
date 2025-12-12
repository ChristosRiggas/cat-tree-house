using UnityEngine;

public class MinigameEnd : MonoBehaviour
{
    public bool hasWon = false;
    private void OnEnable()
    {
        MusicManager.Instance.PlayEndSFX(hasWon);
        Debug.Log("Played end with : " + (hasWon ? "Win" : "Lose"));
    }
}
