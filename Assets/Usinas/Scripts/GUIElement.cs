using UnityEngine;
using System.Collections;
using System;

public class GUIElement : Interactable {

    public override void OnDeselect()
    {
        //Código para trocar para a textura original aqui
        Debug.Log("Desselecionou");
    }

    public override void OnSelected()
    {
        //Código para trocar para a textura de selecionado aqui
        Debug.Log("Selecionou");
    }

    public override void OnTriggerPress(Transform player)
    {
        Debug.Log("Clicou!");
    }

    public override bool OnTriggerRelease(Transform player)
    {
        Debug.Log("Trigger released");
        return true;
    }


}
