using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class VRPlayer_Controller : MonoBehaviour {

    public float speed = 2f;
    public float rotSpeed = 20f;
    public bool canMove;
    private Vector3 playerVelocity = Vector3.zero;
    private CharacterController playerController;

    public VRWand_Controller rightWand, leftWand;    

	void Start () {
        canMove = true;
        playerController = GetComponent<CharacterController>();
	}
	
    void FixedUpdate()
    {
        float rotFromRight = rightWand.rotInput;
        float rotFromLeft = leftWand.rotInput;

        transform.Rotate(Vector3.up * Time.fixedDeltaTime * rotSpeed* ((rotFromRight==0)?rotFromLeft:rotFromRight));

        float walkFromRight = rightWand.walkInput;
        float walkFromLeft = leftWand.walkInput;

        Vector3 fwd = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        transform.Translate(fwd * Time.fixedDeltaTime * speed * ((walkFromRight == 0) ? walkFromLeft : walkFromRight));
    }

}
