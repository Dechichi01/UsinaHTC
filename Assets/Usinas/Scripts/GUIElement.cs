using UnityEngine;
using System.Collections;
using System;

public class GUIElement : Interactable {

    public EventController scriptController;

    protected override void Start()
    {
        base.Start();
        scriptController = GameObject.Find("objs").GetComponent<EventController>();
    }

    public override void OnDeselect()
    {
        //Código para trocar para a textura original aqui
        /*Debug.Log("Desselecionou");
        if (this.name == "UI_BTN")
        {
            Debug.Log("Entrou");
            this.GetComponent<Renderer>().material.color = new Color32(167, 167, 167, 255);
        }*/
    }

    public override void OnSelected()
    {
        //Código para trocar para a textura de selecionado aqui
        //Debug.Log("Selecionou");
        if (this.name == "UI_BTN")
        {
            this.GetComponent<Renderer>().material.color = new Color32(200, 200, 200, 255);
        }
    }

    public override void OnTriggerPress(Transform player)
    {
        if (this.name == "UI_BTN")
        {
            //Debug.Log("Clicou!");
            scriptController.manutencaoTuto();
        }

        if (this.tag == "Manutencao")
        {
            Debug.Log("Clicou!");
            scriptController.Manutencao();
        }
    }

    public override bool OnTriggerRelease(Transform player)
    {
        //Debug.Log("Trigger released");
        return true;
    }


}
