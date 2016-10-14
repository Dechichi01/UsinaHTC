using UnityEngine;
using System.Collections;

public class animation_controller : MonoBehaviour {

    Animation animBraco;
    Animation animPainel;

	// Use this for initialization
	void Start () {

        animPainel = GameObject.Find("painel_final").GetComponent<Animation>();
        animBraco = GameObject.Find("Hand").GetComponent<Animation>();

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            animPainel.Play("p_rotate1");
            animBraco.Play("rotate1");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            animPainel.Play("p_abrir_painel");
            animBraco.Play("abrir_painel");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            animPainel.Play("p_idle-rele_do_tap");
            animBraco.Play("idle-rele_do_tap");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            animPainel.Play("p_rele_do_tap-idle");
            animBraco.Play("rele_do_tap-idle");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            animPainel.Play("p_idle-btn_urgencia");
            animBraco.Play("idle-btn_urgencia");
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            animPainel.Play("p_btn_ugencia-idle");
            animBraco.Play("btn_ugencia-idle");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            animPainel.Play("p_idle-tap_medicao");
            animBraco.Play("idle-tap_medicao");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            animPainel.Play("p_tap_medicao-idle");
            animBraco.Play("tap_medicao-idle");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            animPainel.Play("p_idle-rele_tensao");
            animBraco.Play("idle-rele_tensao");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            animPainel.Play("p_rele_tensao-idle");
            animBraco.Play("p_rele_tensao-idle");
        }

    }
}
