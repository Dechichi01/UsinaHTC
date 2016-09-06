using UnityEngine;
using System.Collections;

public class Pickup : SelectableObject {


    Vector3 initialPosition;
    float approximationTime = 3f;
    float moveAwayTime = 1.2f;

    private float initialScale;
    public float onHandsScale = 0.1f;

    [HideInInspector]
    public bool onHand = false;

    override protected void Start()
    {
        base.Start();
        initialPosition = transform.position;

        initialScale = transform.localScale.x;
    }

    public override void OnTriggerPress(Transform player)
    {
        if (canInteract)
            transform.parent = player;

        Approximate();
    }

    public override bool OnTriggerRelease(Transform player)
    {
        if (canInteract)
        {
            transform.parent = null;
            ChangeToBaseShader();
            MoveAway();
            return true;
        }
        return false;
    }

    public void Approximate()
    {
        if (canInteract && !onHand)
        {
            ChangeToBaseShader();
            StartCoroutine(PerformApproximate());
        }
    }

    public void MoveAway()
    {
        if (canInteract && onHand)
            StartCoroutine(PerformMoveAway());
    }

    IEnumerator PerformApproximate()
    {
        Transform targetChild = transform.parent.FindChild("Model").GetChild(1);

        canInteract = false;
        onHand = true;

        float moveVelocity = 1 / approximationTime;
        float percent = 0;
        Vector3 startPosition = transform.position;
        while (percent < 1)
        {
            //position
            percent += Time.deltaTime * moveVelocity;
            transform.position = Vector3.Lerp(transform.position, targetChild.position, percent);
            //scale
            float size = Mathf.Lerp(transform.localScale.x, onHandsScale, percent);
            transform.localScale = new Vector3(size, size, size);

            yield return null;
        }
        transform.position = targetChild.position;
        transform.localScale = new Vector3(onHandsScale, onHandsScale, onHandsScale);
        canInteract = true;

    }

    IEnumerator PerformMoveAway()
    {
        canInteract = false;
        onHand = false;
        float moveVelocity = 1 / moveAwayTime;
        float percent = 0;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = initialPosition;
        while (percent <1)
        {
            //position
            percent += Time.deltaTime * moveVelocity;
            transform.position = Vector3.Lerp(startPosition, endPosition, percent);
            //rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, percent);

            //size
            float size = Mathf.Lerp(onHandsScale, initialScale, percent);
            transform.localScale = new Vector3(size, size, size);

            yield return null;
        }
        transform.position = endPosition;
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
        canInteract = true;
    }


}
