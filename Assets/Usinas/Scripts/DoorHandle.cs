using UnityEngine;
using System.Collections;
using System;

public class DoorHandle : Interactable
{
    public override void OnTriggerPress(Transform player)
    {
        transform.parent.rotation = transform.parent.rotation * Quaternion.Euler(0f, 180f, 0f);
    }

    public override bool OnTriggerRelease(Transform player)
    {
        return true;
    }
}
