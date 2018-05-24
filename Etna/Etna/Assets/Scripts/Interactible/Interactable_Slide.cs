using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Slide : Interactable {

    [Tooltip("VaultStart and VaultEnd indicate the distance the player travels when interacting with this object. (If the player stands in VaultStart, he will travel along the difference Vector between VaultStart and VaultEnd untill he is at the perpendicular of VaultEnd. Opposite if standing in VaultEnd.)")]
    public BoxCollider SlideStart;
    public BoxCollider SlideEnd;
    private Vector2 SlideAngle;
    private float maxAngleDiff = 0.40f;
    private float speedThreshold = 2f;

    // Use this for initialization
    void Start () {
        SlideAngle = new Vector2(SlideEnd.transform.position.x - SlideStart.transform.position.x, SlideEnd.transform.position.z - SlideStart.transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact(PlayerMovement target)
    {
        Collider[] allOverlappingColliders = Physics.OverlapBox(SlideStart.bounds.center, SlideStart.bounds.extents, SlideStart.transform.rotation);

        foreach (Collider collidedObject in allOverlappingColliders)
        {
            if (collidedObject.GetComponent<PlayerMovement>() == target)
            {
                if (GetAngleDifference(new Vector2(target.GetRigidBody().velocity.x, target.GetRigidBody().velocity.z), SlideAngle) >= 1 - maxAngleDiff || target.GetRigidBody().velocity.magnitude <= speedThreshold)
                {
                    target.Slide(SlideStart.transform.position, SlideEnd.transform.position);
                    Debug.Log("Get slid under mate.");
                    return;
                }
            }
        }

        allOverlappingColliders = Physics.OverlapBox(SlideEnd.bounds.center, SlideEnd.bounds.extents, SlideEnd.transform.rotation);

        foreach (Collider collidedObject in allOverlappingColliders)
        {
            if (collidedObject.GetComponent<PlayerMovement>() == target)
            {
                if (GetAngleDifference(new Vector2(target.GetRigidBody().velocity.x, target.GetRigidBody().velocity.z), SlideAngle) <= -1 + maxAngleDiff || target.GetRigidBody().velocity.magnitude <= speedThreshold)
                {
                    target.Slide(SlideEnd.transform.position, SlideStart.transform.position);
                    Debug.Log("Get slid under from the back mate.");
                    return;
                }
            }
        }
    }
}
