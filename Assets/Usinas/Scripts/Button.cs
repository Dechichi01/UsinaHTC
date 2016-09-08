using UnityEngine;
using System.Collections;
using System;

public class Button : Interactable {

    bool turnedOn = false;
    public Color turnedOffColor;
    public Color turnedOnColor;

    protected override void Start()
    {
        base.Start();
        GetComponent<MeshRenderer>().material.color = turnedOn ? turnedOnColor : turnedOffColor;
    }

    public override void OnTriggerPress(Transform player)
    {
        turnedOn = !turnedOn;
        GetComponent<MeshRenderer>().material.color = turnedOn ? turnedOnColor : turnedOffColor;
    }

    public override bool OnTriggerRelease(Transform player)
    {
        return true;
    }

}
