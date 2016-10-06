using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof (Animation))]
public class TwoStateInteractable : SelectableObject {

    private Animation anim;
    public string turnOnAnimName;
    public string turnOffAnimName;

    bool turnedOn = false;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animation>();
    }

    public override void OnTriggerPress(Transform player)
    {
        if (turnedOn) anim.Play(turnOffAnimName);
        else anim.Play(turnOnAnimName);

        Debug.Log(anim[turnOnAnimName].name);
    }

    public override bool OnTriggerRelease(Transform player)
    {
        return true;
    }
	
}
