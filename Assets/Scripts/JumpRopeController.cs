using UnityEngine;

public class JumpRopeController : MonoBehaviour
{
    public Animator ropeAnimator;
    public PlayerJumpController player;
    public SpriteRenderer ropeSpriteRenderer;
    public float speedIncrease = 0.01f;
    public SpriteRenderer jumpLabel;
    private float currentSpeed = 1f;
    

    private bool canJump;
    private bool hasJumped;


    private void Start()
    {
        ropeSpriteRenderer = GetComponent<SpriteRenderer>();
        ropeSpriteRenderer.sortingOrder = 0;
    }
    private void Update()
    {
        // Detect click
        if (Input.GetMouseButtonDown(0))
        {
            if (!canJump)
            {
                return;
            }
            player.Jump();
            hasJumped = true;
            IncreaseSpeed();
        }
    }

    private void IncreaseSpeed()
    {
        currentSpeed += speedIncrease;
        ropeAnimator.speed = currentSpeed;
    }

    // Called by animation events
    public void EnableJumpWindow() => JumpEnabler();
    public void DisableJumpWindow() => JumpCheck();

    private void JumpEnabler() 
    {
        canJump = true;
        jumpLabel.enabled = true;
        ropeSpriteRenderer.sortingOrder = 1;
    }

    private void JumpCheck()
    {
        canJump = false;
        jumpLabel.enabled = false;
        if (!hasJumped)
        {
            player.FailJump();
            currentSpeed = 0f;
            ropeAnimator.speed = currentSpeed;
        }
        hasJumped = false;
    }
}
