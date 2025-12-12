using System;
using System.Collections;
using UnityEngine;

public class JumpRopeController : MonoBehaviour
{
    public Animator ropeAnimator;
    public PlayerJumpController player;
    public SpriteRenderer ropeSpriteRenderer;
    public float speedIncrease = 0.01f;
    public SpriteRenderer jumpLabel;

    public float startDelay = 2f;
    private float currentSpeed = 1f;


    private bool canJump;
    private bool hasJumpedEarly;
    private bool hasJumped;
    private bool hasFailed;

    private float spamCounter = 0;

    private float delay = 0f;

    private bool hasStarted = false;


    private void Start()
    {
        ropeSpriteRenderer = GetComponent<SpriteRenderer>();
        ropeSpriteRenderer.sortingOrder = 0;
        jumpLabel.enabled = false;
        ropeAnimator.speed = 0f;
        MusicManager.Instance.PlayWaitSFX();
        StartCoroutine(StartGameAfterDelay(startDelay));
    }
    private void Update()
    {

        if (hasFailed || !hasStarted) return;
        // Detect click
        if (Input.GetMouseButtonDown(0))
        {
            if (!canJump)
            {
                if (!hasJumpedEarly)
                {
                    hasJumpedEarly = true;
                    delay += player.EarlyJump();
                    StartCoroutine(ResetEarlyJump());
                }
                else
                {
                    spamCounter += 1;
                }
            }
            if (hasJumpedEarly || spamCounter>=3) 
            {
                return;
            }
            player.Jump();
            canJump = false;
            hasJumped = true;
            IncreaseSpeed();
        }
    }

    IEnumerator StartGameAfterDelay(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);
        hasStarted = true;
        ropeAnimator.speed = currentSpeed;
    }

    IEnumerator ResetEarlyJump()
    {
        yield return new WaitForSeconds(delay);
        hasJumpedEarly = false;
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
            hasFailed = true;
            currentSpeed = 0f;
            ropeAnimator.speed = currentSpeed;
        }
        hasJumped = false;
    }
}
