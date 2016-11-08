using UnityEngine;
using System.Collections;
using System;

public class TwoStateInteractable : SelectableObject {

    private Animation anim;
    public AnimationClip turnOnAnim;
    public AnimationClip turnOffAnim;
    private PanelController panelCtrl;

    [SerializeField]
    private HandController rightHand;

    //[HideInInspector]
    public bool turnedOn = false;

    protected override void Start()
    {
        base.Start();
        //anim = transform.root.GetComponent<Animation>();
        anim = GetComponent<Animation>();
        panelCtrl = FindObjectOfType<PanelController>();
    }

    public override void OnTriggerPress(Transform player)
    {
        if (turnedOn)
        {
            turnedOn = false;
            panelCtrl.ChangeState(this.name, turnedOn);
            string[] animNames = new string[1];
            animNames[0] = turnOffAnim.name;
            rightHand.PerformAnimation(transform.parent, anim, animNames);
        }
        else
        {
            turnedOn = true;
            panelCtrl.ChangeState(this.name, turnedOn);
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
