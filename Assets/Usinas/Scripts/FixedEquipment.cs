using UnityEngine;
using System.Collections;

public class FixedEquipment : SelectableObject {

    public Transform arrivalLocation;

    public float moveTowardsTime = 3f;
    // Use this for initialization
    override protected void Start () {
        base.Start();
        //arrivePosition = transform.FindChild("PlayerPosition").position;
	}

    public override void OnTriggerPress(Transform controller)
    {
        Transform player = controller.parent;
        StartCoroutine(BringPlayer(player.position, arrivalLocation.position, player));
    }

    public override bool OnTriggerRelease(Transform player)
    {
        return true;
    }

    IEnumerator BringPlayer(Vector3 start, Vector3 end, Transform player)
    {
        VRPlayer_Controller playerCtrl = player.GetComponent<VRPlayer_Controller>();
        playerCtrl.canMove = false;
        
        float percent = 0;
        float moveTowardsSpeed = 1 / moveTowardsTime;


        while (percent <= 1)
        {
            percent += (Time.deltaTime * moveTowardsSpeed);
            player.position = Vector3.Lerp(start, end, percent);
            yield return null;
        }

        Debug.Log(player.position);


        playerCtrl.canMove = true;
    }
}
