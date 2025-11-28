using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ValidatePlusButtons : MonoBehaviour
{
    [Header("Raycast setup (arrays must match length)")]
    public Transform[] attachPoints;
    public Canvas[] plusButtonCanvas;
    public Vector2[] vectorDirection;
    public float[] distance;
    public Vector2[] offset;

    [Header("Layer mask for objects that should block the ray")]
    public LayerMask blockMask;

    [Header("Ignore settings")]
    [Tooltip("If true, hits on this GameObject or any of its children will be ignored.")]
    public bool ignoreSelfAndChildren = true;

    private RaycastHit2D[] hitResults = new RaycastHit2D[8];

    private Collider2D collider2D;
    private GameObject[] upgradeButtons;
    private SnapZoneData snapZoneData;

    private void Awake()
    {
        snapZoneData = GetComponentInChildren<SnapZoneData>();
        collider2D = GetComponent<Collider2D>();
        //collider2D.enabled = false; // Disable own collider to prevent self-blocking by default
        upgradeButtons = GameObject.FindGameObjectsWithTag("UpgradeButton");
    }

    void Update()
    {
        ValidatePlusButton();
        UpdateBasedOnUpgradeMode();
    }

    public void UpdateBasedOnUpgradeMode()
    {
        if (UpgradePartManager.Instance != null && !UpgradePartManager.Instance.upgradeMode)
        {
            if(snapZoneData != null)
                snapZoneData.gameObject.SetActive(true);

            collider2D.enabled = false; // Disable own collider to prevent self-blocking
            foreach (var button in upgradeButtons)
            {
                if (button != null)
                    button.SetActive(false);
            }
        }
        else
        {
            if(snapZoneData != null)
                snapZoneData.gameObject.SetActive(false);

            collider2D.enabled = true; // Enable own collider to allow self-blocking
            foreach (var button in upgradeButtons)
            {
                if (button != null)
                    button.SetActive(true);
            }
        }
    }

    public void ValidatePlusButton()
    {
        int count = Mathf.Min(
            attachPoints?.Length ?? 0,
            plusButtonCanvas?.Length ?? 0,
            vectorDirection?.Length ?? 0,
            distance?.Length ?? 0,
            offset?.Length ?? 0
        );

        if (count == 0)
            return;

        for (int i = 0; i < count; i++)
        {
            Transform point = attachPoints[i];

            Canvas canvas = plusButtonCanvas[i];

            if (point == null || canvas == null)
                continue;

            // Get the child button (search inactive too)
            Button btn = canvas.GetComponentInChildren<Button>(true);

            // If no button, skip (but do NOT exit the entire loop)
            if (btn == null)
            {
                Debug.LogWarning($"Canvas {canvas.name} has no Button child.");
                continue;
            }

            // If button itself is inactive → canvas should be hidden
            if (!btn.gameObject.activeSelf)
            {
                canvas.enabled = false;
                continue;
            }

            Vector2 dir = (vectorDirection[i] == Vector2.zero)
                ? Vector2.up
                : vectorDirection[i].normalized;

            float dist = Mathf.Max(0.01f, distance[i]);
            Vector2 origin = (Vector2)point.position + offset[i];

            // Raycast
            int hits = Physics2D.RaycastNonAlloc(origin, dir, hitResults, dist, blockMask);

            bool foundBlocking = false;

            for (int h = 0; h < hits; h++)
            {
                RaycastHit2D hit = hitResults[h];

                if (hit.collider == null)
                    continue;

                if (ignoreSelfAndChildren)
                {
                    if (hit.collider.gameObject == gameObject ||
                        hit.collider.transform.IsChildOf(transform))
                    {
                        continue; // ignore self
                    }
                }

                // Valid blocking hit
                foundBlocking = true;
                //Debug.Log($"PlusButton[{i}] blocked by {hit.collider.name}");
                break;
            }

            // Enable/disable visual
            //canvas.enabled = !foundBlocking;
            //btn.enabled = !foundBlocking;

            // Instead of disabling destory
            if(foundBlocking)
                Destroy(canvas.gameObject);

            // Debug ray
            Debug.DrawRay(origin, dir * dist, foundBlocking ? Color.red : Color.green);
        }
    }

    // --- Gizmo Debugging ---
    void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (attachPoints == null) return;

        int count = attachPoints.Length;

        for (int i = 0; i < count; i++)
        {
            if (attachPoints[i] == null)
                continue;

            Vector2 dir = (vectorDirection != null && i < vectorDirection.Length && vectorDirection[i] != Vector2.zero)
                ? vectorDirection[i].normalized
                : Vector2.up;

            float dist = (distance != null && i < distance.Length)
                ? distance[i]
                : 1f;

            Vector2 off = (offset != null && i < offset.Length)
                ? offset[i]
                : Vector2.zero;

            Vector2 origin = (Vector2)attachPoints[i].position + off;

            // Show origin (cyan)
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(origin, 0.025f);

            // Main ray line (yellow)
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(origin, origin + dir * dist);

            // End point
            Gizmos.DrawSphere(origin + dir * dist, 0.03f);
        }
#endif
    }
}
