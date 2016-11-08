using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PanelController : MonoBehaviour {

    public List<Interactable> panelComponents;

    public Dictionary<string, Interactable> panelComponentsDict = new Dictionary<string, Interactable>();

    void Awake()
    {
        foreach (Interactable component in panelComponents)
            panelComponentsDict.Add(component.name, component);

        //GetStateBtn(panelComponentsDict["btn_tensao3"]);
        //SetStateBtn(panelComponentsDict["btn_tensao3"], false);

        if (GetStateBtn(panelComponentsDict["btn_tensao3"]) == false)
        {
            Debug.Log("Ok");
        }
        
    }

	public void ChangeState(string name, bool state)
    {
        TwoStateInteractable obj = (TwoStateInteractable)panelComponentsDict[name];

        if (obj != null) SetStateBtn(panelComponentsDict[name], state);
        else SetStateVolume(panelComponentsDict[name], state);

        Debug.Log("Estado Btn 3: " + GetStateBtn(panelComponentsDict["btn_tensao3"]));
    }

    bool GetStateBtn(Interactable interactable)
    {
        TwoStateInteractable obj = (TwoStateInteractable)interactable;

        if (obj != null) return obj.turnedOn;

        return false;
    }

    void SetStateBtn(Interactable interactable, bool state)
    {
        TwoStateInteractable obj = (TwoStateInteractable)interactable;

        if (obj != null) obj.turnedOn = state;
    }

    bool GetStateVolume(Interactable interactable)
    {
        VolumeController obj = (VolumeController)interactable;

        if (obj != null) return obj.turnedOn;

        return false;
    }

    void SetStateVolume(Interactable interactable, bool state)
    {
        VolumeController obj = (VolumeController)interactable;

        if (obj != null) obj.turnedOn = state;
    }
}
