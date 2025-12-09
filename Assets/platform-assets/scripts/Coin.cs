using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bool gain = animator.GetBool("gain");
            if (PlatformGameManager.Instance != null && !gain)
            {
                animator.SetBool("gain", true);
                PlatformGameManager.Instance.CollectCoin(coinValue);
            }
             
        }
    }
    public void DestroyCoin()
    {
        Destroy(gameObject);
    }
}