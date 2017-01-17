using UnityEngine;
using System.Collections;
using System;

public class TwoStateInteractable : SelectableObject {

    protected Animation anim;
    public AnimationClip turnOnAnim;
    public AnimationClip turnOffAnim;
    private PanelController panelCtrl;

    [SerializeField]
    protected HandController rightHand;

    [SerializeField]
    protected VRWand_Controller wand;

    protected EventController scriptController;

    //[HideInInspector]
    public bool turnedOn = false;

    protected override void Start()
    {
        base.Start();
        //anim = transform.root.GetComponent<Animation>();
        anim = GetComponent<Animation>();
        panelCtrl = FindObjectOfType<PanelController>();
        scriptController = GameObject.Find("objs").GetComponent<EventController>();
    }

    public override void OnTriggerPress(Transform controllerT)
    {
        controllerT.GetComponent<VRWand_Controller>().ToggleLineRenderer();
        if (turnedOn)
        {
            turnedOn = false;
            panelCtrl.ChangeState(this.name, turnedOn);
            string[] animNames = new string[2];
            animNames[0] = turnOffAnim.name;
            animNames[1] = "reversed";
            rightHand.PerformAnimation(transform.parent, anim, animNames);
        }
        else
        {
            turnedOn = true;
            panelCtrl.ChangeState(this.name, turnedOn);
            string[] animNames = new string[2];
            animNames[0] = turnOnAnim.name;
            animNames[1] = "direct";
            rightHand.PerformAnimation(transform.parent, anim, animNames);
        }

        scriptController.Manutencao();
    }

    public override bool OnTriggerRelease(Transform player)
    {
        return true;
    }

    public void ToggleLineRenderer()
    {
        wand.ToggleLineRenderer();
    }
	
}
