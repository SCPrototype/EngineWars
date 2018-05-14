using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public KeyCode interactKey;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public virtual void Interact(PlayerMovement target)
    {
        Debug.Log("Empty interactable.");
    }

    public virtual KeyCode GetInteractKey()
    {
        return interactKey;
    }

    public float Dot(Vector2 origin, Vector2 target)
    {
        Vector2 normalizedTarget = target.normalized;
        float dot = origin.x * target.x + origin.y * target.y;
        return dot;
    }

    public float GetAngleDifference(Vector2 origin, Vector2 target)
    {
        float dotProduct = Dot(origin, target);
        float angleDiff = dotProduct / (origin.magnitude * target.magnitude);
        return angleDiff;
    }
}
