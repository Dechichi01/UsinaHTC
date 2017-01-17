using UnityEngine;
using System.Collections;

public class VRWand_Controller : MonoBehaviour {

    public LayerMask layerMask;
    public Transform aimTargetPrefab;
    public HandController hand;

    public Transform lineRenderer;
    public Transform pickupHolder;

    private Transform aimTargetInstance;
    private bool castRay = true;

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    [HideInInspector]
    public float rotInput;
    [HideInInspector]
    public float walkInput;

    private VRPlayer_Controller playerController;
    private Transform controllerT;
    private Transform childPickUp;
    private Interactable currentSelectedObject;

    bool wasTriggerReleased = true;//Used by OnTriggerStay

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

        if (castRay && (childPickUp == null || (childPickUp.GetComponent<Pickup>() != null && !childPickUp.GetComponent<Pickup>().onHand)) )
            CastRay(lineRenderer.forward, 50, Color.red, true);
 

        if (controller.GetPressUp(VRInput.triggerButton))
        {
            if (childPickUp != null)
            {
                if(childPickUp.GetComponent<Interactable>().OnTriggerRelease(controllerT))
                    childPickUp = null;
            }
        }

        if (controller.GetPressDown(VRInput.gripButton))//Enable/disable raycasting
        {
            castRay = !castRay;
            lineRenderer.gameObject.SetActive(false);//disable line renderer
            aimTargetInstance.gameObject.SetActive(false);//disable target
        }

        if (controller.GetPressUp(VRInput.triggerButton))
            wasTriggerReleased = true;

        Vector2 input = controller.GetAxis(VRInput.trackPadAxis);
        rotInput = input.x;
        if (!controller.GetPress(VRInput.padButton) || Mathf.Abs(rotInput) < 0.3f)
            rotInput = 0f;

        walkInput = input.y;
        if (!controller.GetPress(VRInput.padButton) || Mathf.Abs(walkInput) < 0.3f)
            walkInput = 0f;
            
    }


    void CastRay(Vector3 direction, float maxDistance, Color color, bool drawRay =false)
    {
        GameObject lineRendererGO = lineRenderer.gameObject;
        if (!lineRendererGO.activeSelf)
            lineRendererGO.SetActive(true);

        Vector3 start = lineRendererGO.transform.position;
        Vector3 end = lineRendererGO.transform.position + direction * maxDistance;
        RaycastHit hit;
        if (Physics.Raycast(new Ray(start, direction), out hit, maxDistance, layerMask))
        {
            Interactable intObj = hit.collider.transform.GetComponent<Interactable>();

            if(intObj != null)
            {
                //Disable target
                aimTargetInstance.gameObject.SetActive(false);
                intObj.OnSelected();

                if (currentSelectedObject != null && currentSelectedObject != intObj)
                {
                    currentSelectedObject.OnDeselect();
                }

                currentSelectedObject = intObj;

                if (controller.GetPressDown(VRInput.triggerButton) && hit.collider.transform.parent != controllerT)
                {
                    if (hit.collider.gameObject.GetComponent<FixedEquipment>() != null)
                    {
                        //castRay = !castRay;
                        //lineRendererGO.SetActive(false);//disable line renderer
                        aimTargetInstance.gameObject.SetActive(false);//disable targe
                    }

                    currentSelectedObject.OnTriggerPress(controllerT);
                    childPickUp = currentSelectedObject.transform;
                    lineRendererGO.SetActive(false);
                }
            }
            else if (hit.collider.CompareTag("WalkableGrid"))
            {
                //Deselect currentObject, if any
                if (currentSelectedObject!=null)
                {
                    currentSelectedObject.OnDeselect();
                    currentSelectedObject = null;
                }
                aimTargetInstance.position = hit.point;
                aimTargetInstance.gameObject.SetActive(true);

                if (controller.GetPressDown(VRInput.triggerButton))
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
                currentSelectedObject.OnDeselect();
                currentSelectedObject = null;
            }
        }

        LineRenderer lr = lineRendererGO.transform.GetComponent<LineRenderer>();
        lr.SetColors(color, color);
        lr.SetWidth(0.002f, 0.002f);
        lr.SetPosition(0, lineRendererGO.transform.position);
        lr.SetPosition(1, lineRendererGO.transform.position + direction*maxDistance);
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

    public void ToggleLineRenderer()
    {
        lineRenderer.gameObject.SetActive(!lineRenderer.gameObject.activeSelf);
        castRay = !castRay;
    }

    public static class VRInput
    {
        public static Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
        public static Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
        public static Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
        public static ulong padButton = SteamVR_Controller.ButtonMask.Touchpad;

        public static Valve.VR.EVRButtonId trackPadAxis = Valve.VR.EVRButtonId.k_EButton_Axis0;

    }
}
