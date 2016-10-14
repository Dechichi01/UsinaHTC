using UnityEngine;
using System.Collections;

public class Panel : FixedEquipment {

    protected override void Start()
    {
        base.Start();
    }

    public override void OnTriggerPress(Transform controller)
    {
        if (!canInteract) return;
        base.OnTriggerPress(controller);
        canInteract = false;
        controller.GetComponent<VRWand_Controller>().hand.ControllerOff(controller.GetComponent<VRWand_Controller>());//hand not holding controller anymore
        controller.FindChild("Model").gameObject.SetActive(false);
    }

    protected override IEnumerator BringPlayer(Vector3 startPos, Quaternion startRot, Vector3 endPos, Quaternion endRot, Transform player)
    {
        yield return base.BringPlayer(startPos, startRot, endPos, endRot, player);
        player.GetComponent<VRPlayer_Controller>().canMove = false;
    }

}
