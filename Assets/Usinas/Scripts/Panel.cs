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

    public override void OnTriggerPress(Transform controller)
    {
        if (!canInteract) return;

        GetComponent<BoxCollider>().enabled = false;
        controller.GetComponent<VRWand_Controller>().hand.ControllerOff(controller.GetComponent<VRWand_Controller>());//hand not holding controller anymore
        controller.FindChild("Model").gameObject.SetActive(false);

        if (!open)
        {
            string[] animations = new string[2];
            animations[1] = waitHandAnim.name;
            animations[0] = openAnim.name;
            controller.GetComponent<VRWand_Controller>().hand.PerformAnimation(transform, anim, animations);
            open = true;
        }
        else
        {
            string[] animations = new string[2];
            animations[1] = waitHandAnim.name;
            animations[0] = closeAnim.name;
            controller.GetComponent<VRWand_Controller>().hand.PerformAnimation(transform, anim, animations);
            open = false;
        }
    }

    protected override IEnumerator BringPlayer(Vector3 startPos, Quaternion startRot, Vector3 endPos, Quaternion endRot, Transform player)
    {
        yield return base.BringPlayer(startPos, startRot, endPos, endRot, player);
        player.GetComponent<VRPlayer_Controller>().canMove = false;
    }

}
