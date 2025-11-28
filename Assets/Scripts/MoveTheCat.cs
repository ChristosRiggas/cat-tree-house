using UnityEngine;
using UnityEngine.UI;

public class MoveTheCat : MonoBehaviour
{
    public static MoveTheCat Instance;

    // Public Properties
    public Transform defaultSnapPoint; // The mandatory starting/cancel point.

    // State Variables
    [HideInInspector] public bool isCarried = false;
    private Vector3 lastSnappedPosition; // The position the object will return to on cancel.
    private Transform colisionSnapPoint = null; // The potential target SnapZone currently being collided with.
    private Camera mainCamera;

    private Animator catAnimator;

    private SnapZoneData currentColisionZoneData;
    private SnapZoneData previousColisionZoneData;
    public SnapZoneData currentZoneData;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        catAnimator = GetComponent<Animator>();

        // Cache the main camera for performance
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            //Debug.LogError("No Main Camera found! Dragging will not work.");
        }

        if (defaultSnapPoint == null)
        {
            //Debug.LogError("The Default Snap Point is not assigned on " + gameObject.name);
            lastSnappedPosition = transform.position;
        }
        else
        {
            transform.position = defaultSnapPoint.position;
            lastSnappedPosition = defaultSnapPoint.position;
        }
    }

    void OnMouseDown()
    {
        if (UpgradePartManager.Instance != null && UpgradePartManager.Instance.upgradeMode)
        {
            return;
        }
        if (isCarried) return; // Prevent picking up if already being dragged (shouldn't happen, but safe)

        //UpgradePartManager.Instance.DisableAllUpgradeButtonsAndColliders();

        isCarried = true;

        AnimatorStateInfo stateInfo = catAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("SleepingNoImage"))
        {
            //Debug.Log("Playing ZZZZ");
            catAnimator.SetTrigger("OpenHouse");
        }

        // Optional: Increase Z-depth to ensure it renders above other non-carried objects
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f); 
    }

    void Update()
    {
        if(UpgradePartManager.Instance != null && UpgradePartManager.Instance.upgradeMode)
        {
            return;
        }
        if (!isCarried) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        transform.position = mousePos;

        if (Input.GetMouseButtonDown(1))
        {
            //CancelPlacement();
            ResetToDefaultLocation();
        }

        if (Input.GetMouseButtonUp(0))
        {
            TryPlaceObject();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Only register a snap point if we are actively carrying the object
        if (isCarried && collision.CompareTag("SnapZone"))
        {
            currentColisionZoneData = collision.GetComponent<SnapZoneData>();
            // colisionSnapPoint = collision.transform;
            colisionSnapPoint = currentColisionZoneData.snapPoint; 
            // Optional: Visually highlight the snap zone here
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SnapZone") && colisionSnapPoint == collision.transform)
        {
            currentColisionZoneData = null; 
            colisionSnapPoint = null;
            // Optional: Remove highlight from the snap zone here
        }
    }

    void TryPlaceObject()
    {
        if (colisionSnapPoint != null)
        {
            if (currentColisionZoneData != null)
            {
                if (currentColisionZoneData.zoneType == SnapZoneType.CryPlace)
                {
                    catAnimator.SetTrigger("Cry");
                }
                else if (currentColisionZoneData.zoneType == SnapZoneType.ClosedHouse)
                {
                    catAnimator.SetTrigger("ClosedHouse");
                }
                else
                {
                    catAnimator.SetTrigger("OpenHouse");
                }

                currentZoneData = currentColisionZoneData;
            }

            if(previousColisionZoneData != null)
                previousColisionZoneData.GetComponent<Collider2D>().enabled = true; // Ensure the previous collider gets re-enabled before assing a new one
            currentColisionZoneData.GetComponent<Collider2D>().enabled = false; // Disable further collisions with this snap zone
            previousColisionZoneData = currentColisionZoneData; // Update previous zone data

            // Snap to the valid point
            transform.position = colisionSnapPoint.position;
            lastSnappedPosition = colisionSnapPoint.position; // Update the new safe return point
            isCarried = false;
            colisionSnapPoint = null; // Clear the temporary snap point
            // Optional: Play a "snap" sound/effect here

        }
        else
        {
            CancelPlacement();
        }

        //UpgradePartManager.Instance.EnableAllUpgradeButtonsAndColliders();  
    }

    public void CancelPlacement()
    {
        // Return to the last successfully snapped position
        if(currentZoneData != null && currentZoneData.zoneType == SnapZoneType.ClosedHouse)
        {
            catAnimator.SetTrigger("ClosedHouse");
        }

        transform.position = lastSnappedPosition;
        isCarried = false;
        colisionSnapPoint = null;
        // Optional: Play a "cancel" sound/effect here
    }

    public void ResetToDefaultLocation()
    {
        transform.position = defaultSnapPoint.position;
        lastSnappedPosition = defaultSnapPoint.position;
        isCarried = false;
        previousColisionZoneData.GetComponent<Collider2D>().enabled = true; // Re-enable previous collider
        currentColisionZoneData = null;
        colisionSnapPoint = null;
        currentZoneData = null;
        catAnimator.SetTrigger("Cry");
    }
}