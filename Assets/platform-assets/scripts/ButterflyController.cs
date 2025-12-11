using UnityEngine;

public class ButterflyController : MonoBehaviour
{
    public float flightSpeed = 1.5f; 
    public float boundaryOffset = 1f; 

    private Rigidbody2D rb;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main; 

        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D !");
        }
    }

    void FixedUpdate()
    {
        if (PlatformGameManager.Instance != null && PlatformGameManager.Instance.isGameOver)
        {
            rb.linearVelocity = Vector2.zero; 
            return;
        }

        rb.linearVelocity = new Vector2(flightSpeed, 0);
    }

    void Update()
    {
        if (mainCamera == null || PlatformGameManager.Instance == null) return;

        float screenWidthInUnits = mainCamera.orthographicSize * mainCamera.aspect * 2f;

        float cameraCenterX = mainCamera.transform.position.x;

        float rightEdgeX = cameraCenterX + (screenWidthInUnits / 2f);

        float butterflyX = transform.position.x;

        if (butterflyX > rightEdgeX + boundaryOffset)
        {
            PlatformGameManager.Instance.GameOver();
        }
    }
}