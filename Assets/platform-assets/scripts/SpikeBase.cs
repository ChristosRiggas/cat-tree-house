using UnityEngine;

public class SpikeBase : MonoBehaviour
{
    public GameObject spikeProjectile;
    private Animator anim;

    private bool hasActivated = false; 

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasActivated)
        {
            ActivateTrap();
        }
    }

    private void ActivateTrap()
    {
        hasActivated = true; 

        if (anim != null)
        {
            anim.SetBool("drop", true);
        }
    }

    public void ReleaseSpike()
    {
        if (spikeProjectile != null)
        {
            spikeProjectile.SetActive(true);
        }
    }
}