using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour {

    private Animation anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animation>();
	}
	
    public void ControllerOn()
    {
        anim.Play("ani_holding_controller");
    }

    public void ControllerOff()
    {
        anim.Play("idle_mao");
    }

	// Update is called once per frame
	void Update () {
	
	}
}
