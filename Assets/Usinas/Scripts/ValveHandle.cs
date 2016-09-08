using UnityEngine;
using System.Collections;
using System;

public class ValveHandle : Interactable {

    private Vector3 turnedOffRot;
    public Vector3 turnedOnRot;
    bool turnedOn = false;

    protected override void Start()
    {
        base.Start();
        turnedOffRot = transform.parent.rotation.eulerAngles;
    }

    public override void OnTriggerPress(Transform player)
    {
        turnedOn = !turnedOn;
        transform.parent.rotation = turnedOn ? Quaternion.Euler(turnedOnRot) : Quaternion.Euler(turnedOffRot);
    }

    public override bool OnTriggerRelease(Transform player)
    {
        return true;
    }
	
}
