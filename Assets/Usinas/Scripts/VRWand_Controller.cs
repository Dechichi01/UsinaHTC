using UnityEngine;
using System.Collections;

public class VRWand_Controller : MonoBehaviour {

    public LayerMask layerMask;
    public Transform aimTargetPrefab;
    private Transform aimTargetInstance;

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    private VRPlayer_Controller playerController;
    private Transform controllerT;
    private Transform childPickUp;
    private SelectableObject currentSelectedObject;

	void Start () {
        playerController = this.transform.parent.GetComponent<VRPlayer_Controller>();
        controllerT = this.transform;
        childPickUp = null;
        trackedObj = GetComponent<SteamVR_TrackedObject>();

        aimTargetInstance = Instantiate(aimTargetPrefab) as Transform;
        aimTargetInstance.gameObject.SetActive(false);
    }
	
	void Update () {
	    if (controller == null)
        {
            Debug.LogError("No tracked object find for this instance");
            return;
        }

        if (childPickUp == null || (childPickUp.GetComponent<Pickup>() != null && !childPickUp.GetComponent<Pickup>().onHand) )
            CastRay(controllerT.GetChild(0).GetChild(0).forward, 50, Color.red, true);
 

        if (controller.GetPressUp(VRInput.triggerButton))
        {
            if (childPickUp != null)
            {
                if(childPickUp.GetComponent<SelectableObject>().OnTriggerRelease(controllerT))
                    childPickUp = null;
            }
        }

        Vector2 moveInput = controller.GetAxis(VRInput.trackPadAxis);
        if (Mathf.Abs(moveInput.x) < 0.3f)
            moveInput.x = 0;
        if (Mathf.Abs(moveInput.y) < 0.3f)
            moveInput.y = 0;

        Vector3 playerMovement = Vector3.ProjectOnPlane(Camera.main.transform.forward,Vector3.up) * moveInput.y + Camera.main.transform.right * moveInput.x;
        playerController.SetPlayerVelocity(playerMovement);
    }

    void CastRay(Vector3 direction, float maxDistance, Color color, bool drawRay =false)
    {
        GameObject lineRenderer = transform.GetChild(0).GetChild(0).gameObject;
        if (!lineRenderer.activeSelf)
            lineRenderer.SetActive(true);

        Vector3 start = lineRenderer.transform.position;
        Vector3 end = lineRenderer.transform.position + direction * maxDistance;
        RaycastHit hit;
        if (Physics.Raycast(new Ray(start, direction), out hit, maxDistance, layerMask))
        {
            if(hit.collider.CompareTag("SelectableObject"))
            {
                //Disable target
                aimTargetInstance.gameObject.SetActive(false);

                SelectableObject obj = hit.collider.transform.GetComponent<SelectableObject>();
                if (currentSelectedObject != obj)
                {
                    if (currentSelectedObject != null)
                        currentSelectedObject.ChangeToBaseShader();

                    currentSelectedObject = obj;
                    currentSelectedObject.ChangeToSelectedShader();
                }

                if (controller.GetPress(VRInput.triggerButton) && hit.collider.transform.parent != controllerT && controllerT.childCount < 2)
                {
                    currentSelectedObject.OnTriggerPress(controllerT);
                    childPickUp = currentSelectedObject.transform;
                    lineRenderer.SetActive(false);
                }
            }
            else if (hit.collider.CompareTag("WalkableGrid"))
            {
                //Deselect currentObject, if any
                if (currentSelectedObject!=null)
                {
                    currentSelectedObject.ChangeToBaseShader();
                    currentSelectedObject = null;
                }
                aimTargetInstance.position = hit.point;
                aimTargetInstance.gameObject.SetActive(true);

                if (controller.GetPress(VRInput.triggerButton))
                {
                    playerController.transform.position = new Vector3(hit.point.x, playerController.transform.position.y, hit.point.z);
                }
            }
        }
        else//If we didn't hit anything, disable target and deselect object
        {
            aimTargetInstance.gameObject.SetActive(false);

            if (currentSelectedObject != null)
            {
                currentSelectedObject.ChangeToBaseShader();
                currentSelectedObject = null;
            }
        }

        LineRenderer lr = lineRenderer.transform.GetComponent<LineRenderer>();
        lr.SetColors(color, color);
        lr.SetWidth(0.002f, 0.002f);
        lr.SetPosition(0, lineRenderer.transform.position);
        lr.SetPosition(1, lineRenderer.transform.position + direction*maxDistance);
    }


    void ReleaseObjects()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<Pickup>() != null)
            {
                child.GetComponent<Pickup>().MoveAway();
                child.parent = null;
            }
        }
    }

    public static class VRInput
    {
        public static Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
        public static Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

        public static Valve.VR.EVRButtonId trackPadAxis = Valve.VR.EVRButtonId.k_EButton_Axis0;

    }
}
