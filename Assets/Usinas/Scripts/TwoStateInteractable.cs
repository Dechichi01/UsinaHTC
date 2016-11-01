using UnityEngine;
using System.Collections;
using System;

public class TwoStateInteractable : SelectableObject {

    private Animation anim;
    public AnimationClip turnOnAnim;
    public AnimationClip turnOffAnim;

    [SerializeField]
    private HandController rightHand;

    //[HideInInspector]
    public bool turnedOn = false;

    protected override void Start()
    {
        base.Start();
        //anim = transform.root.GetComponent<Animation>();
        anim = GetComponent<Animation>();
    }

    public override void OnTriggerPress(Transform player)
    {
        if (turnedOn)
        {
            turnedOn = false;
            string[] animNames = new string[1];
            animNames[0] = turnOffAnim.name;
            rightHand.PerformAnimation(transform.parent, anim, animNames);
        }
        else
        {
            turnedOn = true;
            string[] animNames = new string[1];
            animNames[0] = turnOnAnim.name;
            rightHand.PerformAnimation(transform.parent, anim, animNames);
        }
    }

    public override bool OnTriggerRelease(Transform player)
    {
        return true;
    }
	
}
