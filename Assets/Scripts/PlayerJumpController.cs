using System.Collections;
using UnityEngine;

public class PlayerJumpController : MonoBehaviour
{
    public Animator animator;
    public float jumpheight = 0.3f;
    private float jumpduration;
    private SpriteRenderer spriteRenderer;
    public GameObject gameOverScreen;


    private MusicManager musicManager;

    Vector3 jumpPosition;
    Vector3 landPosition;
    private void Start()
    {
        musicManager = MusicManager.Instance;
        landPosition = transform.position;
        jumpPosition = new Vector3(transform.position.x, transform.position.y + jumpheight, transform.position.z);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Jump()
    {
        animator.Play("PlayerJump", 0, 0f);
        musicManager.PlaySFX();
        jumpduration = animator.GetCurrentAnimatorStateInfo(0).length/2;
        StartCoroutine(JumpRoutine(jumpPosition,jumpduration));
    }

    public void Land()
    {
        jumpduration = animator.GetCurrentAnimatorStateInfo(0).length / 2;
        StartCoroutine(JumpRoutine(landPosition, jumpduration));
        CurrencyManager.Instance.AddCurrency(3);
        Debug.Log("Landing");
    }

    public void FailJump()
    {
        spriteRenderer.sortingLayerID = SortingLayer.NameToID("Foreground");
        spriteRenderer.sortingOrder = 10;
        animator.Play("PlayerFall", 0, 0f);
        gameOverScreen.SetActive(true);
    }

    public void TriggerMove(Vector3 targetPos, float duration)
    {
        StartCoroutine(JumpRoutine(targetPos, duration));
    }

    private IEnumerator JumpRoutine(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target; // snap exactly at end
    }

    internal float EarlyJump()
    {
        animator.Play("PlayerJump", 0, 0f);
        jumpduration = animator.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(JumpRoutine(jumpPosition, jumpduration/2));
        return jumpduration;
    }

    public void LoadMenu()
    {
        //CurrencyManager.Instance.AddCurrency(GetCoinCount());

        GameSceneManager.Instance.ChangeScene("Lobby");
    }
}
