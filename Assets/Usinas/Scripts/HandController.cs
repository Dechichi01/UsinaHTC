using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour {

    public Transform lineRenderer;
    public Transform pickupHolder;
    private Transform previousLineRenderer;
    private Transform previousPickupHolder;

    private Animation anim;

    private Vector3 startLocalPos;
    private Quaternion startLocalRot;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animation>();
        startLocalPos = transform.localPosition;
        startLocalRot = transform.localRotation;
	}
	
    public void ControllerOn(VRWand_Controller wand)
    {
        if (previousLineRenderer != null && previousPickupHolder != null)
        {
            wand.lineRenderer = previousLineRenderer;
            wand.pickupHolder = previousPickupHolder;
        }

        anim.Play("h_idle_controller");
        startLocalPos = transform.localPosition;
    }

    public void ControllerOff(VRWand_Controller wand)
    {
        previousLineRenderer = wand.lineRenderer;
        previousPickupHolder = wand.pickupHolder;

        wand.lineRenderer = lineRenderer;
        wand.pickupHolder = pickupHolder;
        anim.Play("h_idle");
    }

    public void PerformAnimation(Transform panel, Animation componentAnim, string animName)
    {
        StartCoroutine(PositionateHand(panel, componentAnim, animName));
    }

    private string ChangeToControllerAnim(string s1)
    {
        string s2 = s1.Remove(0, 1);
        s2 = s2.Insert(0, "h");
        return s2;
    }

    public IEnumerator PositionateHand(Transform panel, Animation componentAnim, string animName)
    {
        //Move a mão até o painel
        Transform parent = transform.parent;
        transform.parent = null;

        Vector3 delta = panel.position - transform.position;//
        Vector3 start = transform.position;
        Vector3 end = panel.position - delta;

        float percent = 0;
        float speed = 1 / 0.5f;

        while(percent<1)
        {
            percent += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(start, end, percent);
            yield return null;
        }

        transform.position = end;
        //Toca as animações e aguarda elas finalizarem
        string animation = ChangeToControllerAnim(animName);
        componentAnim.Play(animName);
        anim.Play(animation);

        while (anim.IsPlaying(animation)) yield return null;

        //Retorna a mão na posição inicial
        transform.parent = parent;

        start = transform.localPosition;
        end = startLocalPos;

        Quaternion startRot = transform.localRotation;
        Quaternion endRot = startLocalRot;

        percent = 0;
        speed = 1 / 0.5f;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(start, end, percent);
            transform.localRotation = Quaternion.Lerp(startRot, endRot, percent);
            yield return null;
        }

        transform.localPosition = end;
        transform.localRotation = endRot;

        //anim.Play("h_idle_ctrl");

        //PlayAnimations(componentAnim, animName, parent);
    }


	// Update is called once per frame
	void Update () {
	
	}
}
