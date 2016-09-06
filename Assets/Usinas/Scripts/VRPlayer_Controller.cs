using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class VRPlayer_Controller : MonoBehaviour {

    public float speed = 2f;
    public bool canMove;
    private Vector3 playerVelocity = Vector3.zero;
    private CharacterController playerController;

	void Start () {
        canMove = true;
        playerController = GetComponent<CharacterController>();
	}
	
	void Update () {
        if (canMove)
            playerController.Move(playerVelocity * speed*Time.deltaTime);
	}

    public void SetPlayerVelocity(Vector3 velocity)
    {
        playerVelocity = velocity;
    }

}
