using UnityEngine;

public class SpikeProjectile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlatformGameManager.Instance != null)
            {
                PlatformGameManager.Instance.GameOver();
            }
        }
    }

}