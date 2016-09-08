using UnityEngine;
using System.Collections;

public abstract class SelectableObject : MonoBehaviour {

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

    abstract public void OnTriggerPress(Transform player);

    abstract public bool OnTriggerRelease(Transform player);

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
