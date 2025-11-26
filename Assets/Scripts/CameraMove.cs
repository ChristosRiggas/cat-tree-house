using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    [Tooltip("Minimum input strength before the camera moves. (0.1–0.3 recommended).")]
    public float inputThreshold = 0.2f;

    [Header("Camera Borders (World Units)")]
    public float leftLimit = -10f;
    public float rightLimit = 10f;
    public float bottomLimit = -5f;
    public float topLimit = 5f;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");  // arrow keys A/D
        float vertical = Input.GetAxisRaw("Vertical");    // arrow keys W/S

        Vector3 move = Vector3.zero;

        // Horizontal movement if input is above threshold
        if (Mathf.Abs(horizontal) > inputThreshold)
            move.x = Mathf.Sign(horizontal);

        // Vertical movement if input is above threshold
        if (Mathf.Abs(vertical) > inputThreshold)
            move.y = Mathf.Sign(vertical);

        // Move camera
        if (move != Vector3.zero)
            transform.position += move * moveSpeed * Time.deltaTime;

        // Clamp to camera borders
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
        pos.y = Mathf.Clamp(pos.y, bottomLimit, topLimit);
        transform.position = pos;
    }
}