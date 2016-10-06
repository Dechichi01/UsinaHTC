using UnityEngine;
using System.Collections;

public class Panel : FixedEquipment {

    protected override void Start()
    {
        base.Start();
    }

    public override void OnTriggerPress(Transform controller)
    {
        base.OnTriggerPress(controller);
        controller.GetComponent<VRWand_Controller>().hand.ControllerOff();//hand not holding controller anymore
    }

    protected override IEnumerator BringPlayer(Vector3 startPos, Quaternion startRot, Vector3 endPos, Quaternion endRot, Transform player)
    {
        yield return base.BringPlayer(startPos, startRot, endPos, endRot, player);
        player.GetComponent<VRPlayer_Controller>().canMove = false;
    }

}
