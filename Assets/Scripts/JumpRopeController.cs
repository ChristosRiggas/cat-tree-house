using UnityEngine;

public class JumpRopeController : MonoBehaviour
{
    public Animator ropeAnimator;
    public PlayerJumpController player;

    [Range(0f, 1f)] public float perfectWindow = 0.1f;
    [Range(0f, 1f)] public float goodWindow = 0.2f;

    private float currentSpeed = 1f;
    private const float speedIncrease = 0.03f;

    private bool canJump;

    private void Update()
    {
        // Detect click
        if (Input.GetMouseButtonDown(0))
        {
            if (!canJump)
            {
                player.FailJump();   // too early
                return;
            }

            float t = GetRopeNormalizedTime();

            if (Mathf.Abs(t - 0.5f) < perfectWindow)
            {
                player.JumpPerfect();
                IncreaseSpeed();
            }
            else if (Mathf.Abs(t - 0.5f) < goodWindow)
            {
                player.JumpGood();
                IncreaseSpeed();
            }
            else
            {
                player.FailJump();   // too late
            }
        }
    }

    private float GetRopeNormalizedTime()
    {
        AnimatorStateInfo st = ropeAnimator.GetCurrentAnimatorStateInfo(0);
        return st.normalizedTime % 1f;
    }

    private void IncreaseSpeed()
    {
        currentSpeed += speedIncrease;
        ropeAnimator.speed = currentSpeed;
    }

    // Called by animation events
    public void EnableJumpWindow() => canJump = true;
    public void DisableJumpWindow() => canJump = false;
}
