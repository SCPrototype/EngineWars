using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Vault : Interactable {

    [Tooltip("VaultStart and VaultEnd indicate the distance the player travels when interacting with this object. (If the player stands in VaultStart, he will travel along the difference Vector between VaultStart and VaultEnd untill he is at the perpendicular of VaultEnd. Opposite if standing in VaultEnd.)")]
    public BoxCollider VaultStart;
    public BoxCollider VaultEnd;
    private Vector2 VaultAngle;
    private float maxAngleDiff = 0.40f;
    private float speedThreshold = 1f;

    // Use this for initialization
    void Start () {
        VaultAngle = new Vector2(VaultEnd.transform.position.x - VaultStart.transform.position.x, VaultEnd.transform.position.z - VaultStart.transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact(PlayerMovement target)
    {
        Debug.Log(target.GetRigidBody().velocity.magnitude);
        Collider[] allOverlappingColliders = Physics.OverlapBox(VaultStart.bounds.center, VaultStart.bounds.extents, VaultStart.transform.rotation);

        foreach (Collider collidedObject in allOverlappingColliders)
        {
            if (collidedObject.GetComponent<PlayerMovement>() == target)
            {
                if (GetAngleDifference(new Vector2(target.GetRigidBody().velocity.x, target.GetRigidBody().velocity.z), VaultAngle) >= 1 - maxAngleDiff || target.GetRigidBody().velocity.magnitude <= speedThreshold)
                {
                    target.Vault(VaultStart.transform.position, VaultEnd.transform.position);
                    Debug.Log("Get vaulted over dude.");
                    return;
                }
            }
        }

        allOverlappingColliders = Physics.OverlapBox(VaultEnd.bounds.center, VaultEnd.bounds.extents, VaultEnd.transform.rotation);

        foreach (Collider collidedObject in allOverlappingColliders)
        {
            if (collidedObject.GetComponent<PlayerMovement>() == target)
            {
                if (GetAngleDifference(new Vector2(target.GetRigidBody().velocity.x, target.GetRigidBody().velocity.z), VaultAngle) <= -1 + maxAngleDiff || target.GetRigidBody().velocity.magnitude <= speedThreshold)
                {
                    target.Vault(VaultEnd.transform.position, VaultStart.transform.position);
                    Debug.Log("Get vaulted over from the back dude.");
                    return;
                }
            }
        }
    }
}
