using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour {

    public Transform lineRenderer;
    public Transform pickupHolder;
    private Transform previousLineRenderer;
    private Transform previousPickupHolder;

    public AnimationClip controllerOnAnim;
    public AnimationClip controllerOffAnim;

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

        anim.Play(controllerOnAnim.name);
        startLocalPos = transform.localPosition;
    }

    public void ControllerOff(VRWand_Controller wand)
    {
        previousLineRenderer = wand.lineRenderer;
        previousPickupHolder = wand.pickupHolder;

        wand.lineRenderer = lineRenderer;
        wand.pickupHolder = pickupHolder;
        anim.Play(controllerOffAnim.name);
    }

    public void PerformAnimation(Transform panel, Animation componentAnim, string[] animNames, bool isVolumeCtrl = false)
    {
        StartCoroutine(Positionate_HandPanel(panel, componentAnim, animNames, isVolumeCtrl));
    }

    private string ChangeToControllerAnim(string s1)
    {
        string s2 = s1.Remove(0, 1);
        s2 = s2.Insert(0, "h");
        return s2;
    }

    //Used for iteractions that involves animation
    public IEnumerator Positionate_HandPanel(Transform panel, Animation componentAnim, string[] animNames, bool isVolCtrl = false)
    {
        //Move a mão até o painel
        Transform parent = transform.parent;
        transform.parent = null;

        Vector3 start = transform.position;
        Vector3 end = panel.position;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = panel.rotation;

        float percent = 0;
        float speed = 1 / 0.5f;

        //start playing animations
        string turnOnAnimName = animNames[0];
        string animation = ChangeToControllerAnim(turnOnAnimName);
        componentAnim.Play(turnOnAnimName);
        anim.Play(animation);

        while (percent<1)
        {
            percent += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(start, end, percent);
            transform.rotation = Quaternion.Lerp(startRot, endRot, percent);
            yield return null;
        }

        transform.position = end;

        //Wait for animation to finish
        while (anim.IsPlaying(animation)) yield return null;

        Debug.Log(isVolCtrl);
        if (!isVolCtrl) StartCoroutine(Positionate_PanelHand(parent, componentAnim, animNames, isVolCtrl));
        else StartCoroutine(PerformVolumeCtrl(parent, componentAnim, animNames, isVolCtrl));
        
    }

    public IEnumerator Positionate_PanelHand(Transform parent, Animation componentAnim, string[] animNames, bool isVolCtrl = false)
    {
        //Retorna a mão à posição inicial
        transform.parent = parent;

        if (isVolCtrl)
        {
            string releaseAnim = animNames[2];
            string releaseAnim_h = ChangeToControllerAnim(releaseAnim);
            componentAnim.Play(releaseAnim);
            anim.Play(releaseAnim_h);
        }

        Vector3 start = transform.localPosition;
        Vector3 end = startLocalPos;

        Quaternion startRot = transform.localRotation;
        Quaternion endRot = startLocalRot;

        float percent = 0;
        float speed = 1 / 0.5f;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(start, end, percent);
            transform.localRotation = Quaternion.Lerp(startRot, endRot, percent);
            yield return null;
        }

        transform.localPosition = end;
        transform.localRotation = endRot;
    }

    IEnumerator PerformVolumeCtrl(Transform parent, Animation componentAnim, string[] animNames, bool isVolCtrl = false)
    {
        int sign = (animNames[3] == "reversed")?-1:1;

        float initialRotZ = sign*Mathf.Abs(parent.localRotation.z);
        float finalRotZ = initialRotZ + sign*0.5f;

        string rotAnim = animNames[1];
        string rotAnim_h = ChangeToControllerAnim(rotAnim);

        float rot = sign*Mathf.Abs((parent.localRotation * Quaternion.Euler(0f, 0f, -initialRotZ)).z); ;

        float animTime = 0f;
        while (animTime != anim[rotAnim_h].length)
        {
            //Lock CCW rotations and prevents animations jump due to rotation sign change 
            if (sign*parent.localRotation.z <= 0 && !(sign*rot > 0.7f && animTime < 0.1f*anim[rotAnim_h].length)) 
                animTime = Mathf.InverseLerp(initialRotZ, finalRotZ, rot) * anim[rotAnim_h].length;

            componentAnim[rotAnim].speed = 0;
            componentAnim.Play(rotAnim);
            componentAnim[rotAnim].time = (sign ==1)?animTime:(1-animTime);

            anim[rotAnim_h].speed = 0;
            anim.Play(rotAnim_h);
            anim[rotAnim_h].time = (sign == 1) ? animTime : (1 - animTime);

            yield return null;

            rot = sign*Mathf.Abs((parent.localRotation*Quaternion.Euler(0f, 0f, -initialRotZ)).z);
        }

        StartCoroutine(Positionate_PanelHand(parent, componentAnim, animNames, isVolCtrl));

    }
}
