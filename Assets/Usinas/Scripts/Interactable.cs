using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider))]
public abstract class Interactable : MonoBehaviour {

    protected bool canInteract;

    virtual protected void Start()
    {
        gameObject.tag = "Interactable";
        canInteract = true;
    }

    abstract public void OnTriggerPress(Transform player);

    abstract public bool OnTriggerRelease(Transform player);

}
