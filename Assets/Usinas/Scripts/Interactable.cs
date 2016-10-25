using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider))]
public abstract class Interactable : MonoBehaviour {

    [SerializeField]
    protected bool canInteract;

    private LayerMask previousLayer;

    virtual protected void Start()
    {
        previousLayer = LayerMask.NameToLayer("SelectableObject");
        gameObject.tag = "Interactable";
        canInteract = true;
    }

    abstract public void OnTriggerPress(Transform player);

    abstract public bool OnTriggerRelease(Transform player);

    virtual protected void DisableInteractions()
    {
        canInteract = false;
        previousLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    virtual protected void EnableInteractions()
    {
        canInteract = true;
        gameObject.layer = previousLayer;
    }

}
