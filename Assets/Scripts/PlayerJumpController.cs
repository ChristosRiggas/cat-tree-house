using UnityEngine;

public class PlayerJumpController : MonoBehaviour
{
    public Animator animator;

    public void JumpPerfect()
    {
        animator.Play("PlayerJump", 0, 0f);
    }

    public void JumpGood()
    {
        animator.Play("PlayerJump", 0, 0f);
    }

    public void FailJump()
    {
        animator.Play("PlayerFall", 0, 0f);
    }
}
