using UnityEngine;
using System.Collections;

public class SelectableObject : MonoBehaviour {

    protected bool canInteract;

    public Shader selectedShader;
    private Shader[] baseShader;

    virtual protected void Start()
    {
        gameObject.tag = "SelectableObject";
        canInteract = true;

        Material[] materials = transform.GetChild(0).GetComponent<MeshRenderer>().materials;
        baseShader = new Shader[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            baseShader[i] = materials[i].shader;
        }

    }

    virtual public void OnTriggerPress(Transform player)
    {
        Debug.LogWarning("SelectableObject base class. You should not be here!");
    }

    virtual public bool OnTriggerRelease(Transform player)
    {
        Debug.LogWarning("SelectableObject base class. You should not be here!");
        return false;
    }

    public void ChangeToSelectedShader()
    {
        if (canInteract)
        {
            Material[] materials = transform.GetChild(0).GetComponent<MeshRenderer>().materials;
            Renderer renderer = transform.GetChild(0).GetComponent<Renderer>();
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].shader = selectedShader;
                renderer.materials[i].SetColor("_OutlineColor", new Color(0, 255, 0, 1));
            }

        }

    }

    public void ChangeToBaseShader()
    {
        if (canInteract)
        {
            Material[] materials = transform.GetChild(0).GetComponent<MeshRenderer>().materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].shader = baseShader[i];
            }
        }
    }
}
