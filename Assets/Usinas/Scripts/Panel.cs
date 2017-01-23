using UnityEngine;
using System.Collections;

public class Panel : FixedEquipment {

    private Animation anim;

    public AnimationClip waitHandAnim;
    public AnimationClip openAnim;
    public AnimationClip closeAnim;

    private bool open = false;

    protected override void Start()
    {
        anim = GetComponent<Animation>();
        base.Start();
    }

    public override void OnTriggerPress(VRWand_Controller wand)
    {
        if (!canInteract) return;

        GetComponent<BoxCollider>().enabled = false;
        wand.hand.ControllerOff(wand);//hand not holding controller anymore
        wand.transform.FindChild("Model").gameObject.SetActive(false);

        if (!open)
        {
            string[] animations = new string[2];
            animations[1] = waitHandAnim.name;
            animations[0] = openAnim.name;
            wand.hand.PerformAnimation(transform, anim, animations);
            open = true;
        }
        else
        {
            string[] animations = new string[2];
            animations[1] = waitHandAnim.name;
            animations[0] = closeAnim.name;
            wand.hand.PerformAnimation(transform, anim, animations);
            open = false;
        }
    }

    protected override IEnumerator BringPlayer(Vector3 startPos, Quaternion startRot, Vector3 endPos, Quaternion endRot, Transform player)
    {
        yield return base.BringPlayer(startPos, startRot, endPos, endRot, player);
        player.GetComponent<VRPlayer_Controller>().canMove = false;
    }

}
