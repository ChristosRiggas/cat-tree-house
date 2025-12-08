using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
   
    public float moveSpeed = 2f;     
    public float patrolDistance = 5f; 

    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 startPosition;    
    private bool movingRight = true;  

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

  
        startPosition = transform.position;

    }

    void Update()
    {
        float currentX = transform.position.x;

        float rightBoundary = startPosition.x + patrolDistance;
 

        if (movingRight)
        {
            if (currentX >= rightBoundary)
            {
                movingRight = false;
                Flip(); 
            }
        }
        else 
        {
            if (currentX <= startPosition.x)
            {
                movingRight = true;
                Flip(); 
            }
        }
    }

    void FixedUpdate()
    {
        float direction = movingRight ? 1f : -1f;

        rb.linearVelocity = new Vector2(direction * moveSpeed, 0);
    }

    private void Flip()
    {
        Vector3 newScale = transform.localScale;

        newScale.x *= -1;

        transform.localScale = newScale;
    }
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