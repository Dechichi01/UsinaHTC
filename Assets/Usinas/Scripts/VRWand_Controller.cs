using UnityEngine;
using System.Collections;

public class VRWand_Controller : MonoBehaviour {

    #region Variables to be assigned in inspector
    public LayerMask interactMask;
    public HandController hand;
    public Transform pickupHolder;
    #endregion

    #region VR Controller variables
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    #endregion

    #region Private Variables
    private VRInteraction vrInteraction;
    private VRPlayer_Controller playerController;
    private Transform controllerT;
    private Pickup _childPickup = null;
    #endregion

    #region Public Variables
    [HideInInspector]
    public float rotInput;
    [HideInInspector]
    public float walkInput;
    public Pickup childPickup
    {
        get { return _childPickup; }
        set { _childPickup = value; }
    }
    #endregion

    void Start () {

        playerController = transform.parent.GetComponent<VRPlayer_Controller>();
        vrInteraction = GetComponent<VRInteraction>();
        controllerT = transform;
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
	
	void Update () {
	    if (controller == null)
        {
            Debug.LogError("No tracked object find for this instance");
            return;
        }

        vrInteraction.CheckForInteractables(50f);

        if (controller.GetPressDown(VRInput.triggerButton))
            vrInteraction.TriggerPressed(this);

        //Release grabbed object
        if (controller.GetPressUp(VRInput.triggerButton))
            vrInteraction.TriggerReleased(this);

        if (controller.GetPressDown(VRInput.gripButton))//Enable/disable raycasting
            vrInteraction.GripPressed(this);

        //Movement input
        Vector2 input = controller.GetAxis(VRInput.trackPadAxis);
        rotInput = input.x;
        if (!controller.GetPress(VRInput.padButton) || Mathf.Abs(rotInput) < 0.3f)
            rotInput = 0f;

        walkInput = input.y;
        if (!controller.GetPress(VRInput.padButton) || Mathf.Abs(walkInput) < 0.3f)
            walkInput = 0f;
        //            
    }

    public void ToggleLineRenderer()
    {
        if (vrInteraction is VRRayInteraction)
            vrInteraction.GripPressed(this);
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
