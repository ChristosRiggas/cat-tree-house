//using UnityEngine;

//public class SnapZone : MonoBehaviour
//{
//    public bool isHouse = false; // Indicates if this SnapZone is a house
//    public bool isClosedHouse = false; // Indicates if the house is closed
//}

using UnityEngine;

/// <summary>
/// Defines the type and state of a specific SnapZone GameObject.
/// Must be attached to a GameObject that has the "SnapZone" tag and a trigger Collider2D.
/// </summary>
public class SnapZoneData : MonoBehaviour
{
    // Enum to clearly define the type of environment this zone represents.
    // This makes referencing much cleaner than using multiple booleans.
    public SnapZoneType zoneType = SnapZoneType.CryPlace;
    public Transform snapPoint; // Specific point where objects will snap to within this zone
}

/// <summary>
/// Available snap zone types.
/// </summary>
public enum SnapZoneType
{
    CryPlace,
    OpenHouse,
    ClosedHouse,
    SideBed,
    SidePlatform
}
