using UnityEngine;
using System.Collections;

public class VolumeController : TwoStateInteractable {

    public AnimationClip grabAnim;
    public AnimationClip volRotateAnim;
    public AnimationClip releaseAnim;

    protected override void Start()
    {
        base.Start();
        //anim = transform.root.GetComponent<Animation>();
        anim = GetComponent<Animation>();
        scriptController = GameObject.Find("objs").GetComponent<EventController>();
    }

    public override void OnTriggerPress(Transform player)
    {
        wand.ToggleLineRenderer();
        if (turnedOn)
        {
            turnedOn = false;
            string[] animNames = new string[4];
            animNames[0] = grabAnim.name;
            animNames[1] = volRotateAnim.name;
            animNames[2] = releaseAnim.name;
            animNames[3] = "reversed";
            rightHand.PerformAnimation(transform.parent, anim, animNames, true);
        }
        else
        {
            turnedOn = true;
            string[] animNames = new string[4];
            animNames[0] = grabAnim.name;
            animNames[1] = volRotateAnim.name;
            animNames[2] = releaseAnim.name;
            animNames[3] = "direct";
            rightHand.PerformAnimation(transform.parent, anim, animNames, true);
        }

        scriptController.Manutencao();
    }

    public override bool OnTriggerRelease(Transform player)
    {
        return true;
    }

}
