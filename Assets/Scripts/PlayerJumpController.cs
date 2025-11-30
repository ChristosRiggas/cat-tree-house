using System.Collections;
using UnityEngine;

public class PlayerJumpController : MonoBehaviour
{
    public Animator animator;
    public float jumpheight = 0.3f;
    private float jumpduration;
    private SpriteRenderer spriteRenderer;

    Vector3 jumpPosition;
    Vector3 landPosition;
    private void Start()
    {
        landPosition = transform.position;
        jumpPosition = new Vector3(transform.position.x, transform.position.y + jumpheight, transform.position.z);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Jump()
    {
        animator.Play("PlayerJump", 0, 0f);
        jumpduration = animator.GetCurrentAnimatorStateInfo(0).length/2;
        StartCoroutine(JumpRoutine(jumpPosition,jumpduration));
    }

    public void Land()
    {
        jumpduration = animator.GetCurrentAnimatorStateInfo(0).length / 2;
        StartCoroutine(JumpRoutine(landPosition, jumpduration));
        Debug.Log("Landing");
    }

    public void FailJump()
    {
        spriteRenderer.sortingLayerID = SortingLayer.NameToID("Foreground");
        spriteRenderer.sortingOrder = 10;
        animator.Play("PlayerFall", 0, 0f);
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
}
