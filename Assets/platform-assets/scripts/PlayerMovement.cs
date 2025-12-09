using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f; 
    [SerializeField] private float airControlFactor = 0.5f; 

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 1f; 
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private Transform groundCheck; 
    [SerializeField] private float groundCheckRadius = 0.2f; 

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isGrounded;
    private bool hasJumpedInAir = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        CheckIfGrounded();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !hasJumpedInAir)
        {
            HandleJump();
            hasJumpedInAir = true;
        }

        HandleMovement(horizontalInput);
        HandleFlipping(horizontalInput);
        HandleAnimation(rb.linearVelocityX);
    }

    private void HandleJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        animator.SetBool("IsJumping", true);
    }

    private void HandleMovement(float input)
    {
        float targetSpeed = input * moveSpeed;

        if (!isGrounded)
        {
            targetSpeed *= airControlFactor;
        }

        Vector2 velocity = new Vector2(targetSpeed, rb.linearVelocityY);
        rb.linearVelocity = velocity;
    }
    private void CheckIfGrounded()
    {
        const float CAST_DISTANCE = 0.05f;
        Vector2 castDirection = Vector2.down;

        RaycastHit2D hit = Physics2D.CircleCast(
            groundCheck.position,
            groundCheckRadius,
            castDirection,
            CAST_DISTANCE,
            groundLayer
        );

        isGrounded = false;

        if (hit.collider != null)
        {
            Vector2 surfaceNormal = hit.normal;
            float angle = Vector2.Angle(surfaceNormal, Vector2.up);

            const float GROUND_ANGLE_THRESHOLD = 45f;

            if (angle <= GROUND_ANGLE_THRESHOLD)
            {
                isGrounded = true; 
            }
     
        }

        if (isGrounded)
        {
            hasJumpedInAir = false;

            if (animator.GetBool("IsJumping") && rb.linearVelocityY <= 0.001f)
            {
                animator.SetBool("IsJumping", false);
            }
        }
    }
    private void HandleFlipping(float input)
    {
        if (input > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (input < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
    private void HandleAnimation(float currentHorizontalSpeed)
    {
        float absoluteSpeed = Mathf.Abs(currentHorizontalSpeed);
        animator.SetFloat("speed", absoluteSpeed);
    }
}