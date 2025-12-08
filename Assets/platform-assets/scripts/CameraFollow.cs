using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    private float zOffset = -10f;

    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, zOffset);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}