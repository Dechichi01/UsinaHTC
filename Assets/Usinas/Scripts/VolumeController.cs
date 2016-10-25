using UnityEngine;
using System.Collections;

public class VolumeController : SelectableObject {

    private Animation anim;
    public string turnOnAnimName;
    public string turnOffAnimName;

    [SerializeField]
    private HandController rightHand;
    [SerializeField]
    private HandController leftHand;

    bool turnedOn = false;

    protected override void Start()
    {
        base.Start();
        anim = transform.root.GetComponent<Animation>();
    }

    public override void OnTriggerPress(Transform player)
    {
        if (turnedOn)
        {
            //anim.Play(turnOffAnimName);
            //rightHand.PlayAnimation(ChangeToControllerAnim(turnOffAnimName));
        }
        else
        {
            rightHand.PerformAnimation(transform.parent, anim, turnOnAnimName, true);
        }
    }

    public override bool OnTriggerRelease(Transform player)
    {
        return true;
    }
}
