using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventController : MonoBehaviour {

	public Renderer UI_Renderer;
	public Renderer UI_Tuto_Renderer;
    public Renderer UI_Btn;

    private PanelController panelCtrl;
    private GUIElement scriptGUI;
    private HandController scriptHand;

    private Dictionary<string, Interactable> componentsDict;

    public bool[] steps = new bool[20];

	public Texture[] tutorial_panel = new Texture[20];
	public Texture[] tutorial_text = new Texture[20];
		
	public int tutoId = 0; 

	public float fadeSpeed = 0f;

    public bool manutencaoOk = true;

    public bool emergency = false;

    public Material shaderRedOL;

    public Material shaderGreenOL;

    public Material bkpMaterial;
    public Material bkpMaterial2;

    public string objName;

    public int countBtn;

    public bool keepShader;

    public float readSeconds;


	// Use this for initialization
	void Start () {

        tutoId = 0;
        manutencaoOk = true;
        countBtn = 0;
        keepShader = false;
        readSeconds = 4f;

        UI_Renderer = GameObject.Find ("UI_MSG").GetComponent<Renderer> ();
		UI_Tuto_Renderer = GameObject.Find ("UI_TUTO").GetComponent<Renderer> ();
        UI_Btn = GameObject.Find("UI_BTN").GetComponent<Renderer>();

        UI_Btn.material.color = new Color(0.5f, 0.5f, 0.5f, 1);

        panelCtrl = FindObjectOfType<PanelController>();
        componentsDict = panelCtrl.panelComponentsDict;
	}
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(GameObject.Find("UI_BTN").GetComponent<Renderer>().material.color);

        //panelCtrl.SetStateBtn(componentsDict["btn_tensao3"], true);

		if(Input.GetKeyDown(KeyCode.A))
		{
            StartCoroutine(focusObj("btn_tensao3"));
            StartCoroutine(focusObj("btn_tensao2"));
            StartCoroutine(focusObj("btn_tensao1"));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(enableBtn());
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            emergency = true;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            emergency = false;
        }

        print("tutoId: " + tutoId);

    }

	IEnumerator showMessage(Texture textura) 
	{

        UI_Renderer.material.mainTexture = textura;
		Color colorTemp = UI_Renderer.material.color;
		float alphaIn = 0f;

        while (UI_Renderer.material.color.a <= 1)
		{
			UI_Renderer.material.color = new Color (colorTemp.r, colorTemp.g, colorTemp.b, alphaIn);
            //UI_Btn.material.color = new Color(colorTemp.r, colorTemp.g, colorTemp.b, alphaIn);
            yield return new WaitForSeconds(fadeSpeed);
			alphaIn += 0.4f;
        }

        yield return new WaitForSeconds(readSeconds);

        StartCoroutine(hideMessage());
        StartCoroutine(hideImage());


        if (tutoId == 13)
        {
            StartCoroutine(enableBtn());
        }

        if (tutoId == 16)
        {
            emergency = true;
            StartCoroutine(lightWarning());
            StartCoroutine(enableAllBtn());
        }

    }

	IEnumerator hideMessage() 
	{
		Color colorTemp = UI_Renderer.material.color;
		float alphaOut = 1f;
		
		while(UI_Renderer.material.color.a >= 0)
		{
			UI_Renderer.material.color = new Color (colorTemp.r, colorTemp.g, colorTemp.b, alphaOut);
            //UI_Btn.material.color = new Color(colorTemp.r, colorTemp.g, colorTemp.b, alphaOut);
            yield return new WaitForSeconds(fadeSpeed);
			alphaOut -= 0.4f;
		}
	}

	IEnumerator showImage(Texture textura)
	{
//		if(textura != null)
//		{
			UI_Tuto_Renderer.material.mainTexture = textura;
			Color colorTemp = UI_Tuto_Renderer.material.color;
			float alphaIn = 0f;
			
			while(UI_Tuto_Renderer.material.color.a <= 1)
			{
				UI_Tuto_Renderer.material.color = new Color (colorTemp.r, colorTemp.g, colorTemp.b, alphaIn);
			yield return new WaitForSeconds(fadeSpeed);
				alphaIn += 0.4f;
			}
			yield return new WaitForSeconds(1f);
//		}

	}

	IEnumerator hideImage()
	{
		Color colorTemp = UI_Tuto_Renderer.material.color;
		float alphaOut = 1f;
		
		while(UI_Tuto_Renderer.material.color.a >= 0)
		{
			UI_Tuto_Renderer.material.color = new Color (colorTemp.r, colorTemp.g, colorTemp.b, alphaOut);
			yield return new WaitForSeconds(fadeSpeed);
			alphaOut -= 0.4f;
		}
		tutoId++;
	}


	public void manutencaoTuto()
	{
        if(manutencaoOk == true)
        {
            if (UI_Renderer.material.color.a <= 0 && UI_Tuto_Renderer.material.color.a <= 0)
            {
                StartCoroutine(showMessage(tutorial_text[tutoId]));
                StartCoroutine(showImage(tutorial_panel[tutoId]));
            }

            if (UI_Renderer.material.color.a >= 1 && UI_Tuto_Renderer.material.color.a >= 1)
            {
                StartCoroutine(hideMessage());
                StartCoroutine(hideImage());
            }
            
        }
	}

    public void Manutencao()
    {
        if (tutoId == 12)
        {
            if (panelCtrl.GetStateBtn(componentsDict["obj_btn_vol3"]) == true)
            {
                StartCoroutine(lightGreen());
            }
            else
            {
                StartCoroutine(lightRed());
            }
        }

        if (tutoId == 13)
        {
            if (panelCtrl.GetStateBtn(componentsDict["btn_disjuntor4"]) == true)
            {
                StartCoroutine(lightGreen());
            }
            else
            {
                StartCoroutine(lightRed());
            }
        }

        if (tutoId == 14)
        {
            if (panelCtrl.GetStateBtn(componentsDict["btn_tensao1"]) == false)
            {
                restoreMaterial("btn_tensao1");
                StartCoroutine(lightGreen());
            }
            else
            {
                StartCoroutine(lightRed());
            }
        }

        if (tutoId == 15)
        {
            if (panelCtrl.GetStateBtn(componentsDict["obj_btn_vol1"]) == true)
            {
                StartCoroutine(lightGreen());
            }
            else
            {
                StartCoroutine(lightRed());
            }
        }

        if (tutoId == 16)
        {
            countBtn++;

            if (countBtn >= 2)
            {
                if (panelCtrl.GetStateBtn(componentsDict["btn_disjuntor2"]) == true && panelCtrl.GetStateBtn(componentsDict["btn_disjuntor3"]) == true)
                {
                    StartCoroutine(lightGreen());
                    countBtn = 0;
                }
                else
                {
                    StartCoroutine(lightRed());
                }
            }
        }

        if (tutoId == 17)
        {
            if (panelCtrl.GetStateBtn(componentsDict["btn_urgencia"]) == true)
            {
                StartCoroutine(lightGreen());
                emergency = false;
            }
            else
            {
                StartCoroutine(lightRed());
            }
        }

        if (tutoId == 18)
        {
            countBtn++;
            if (countBtn >= 3)
            {
                if (panelCtrl.GetStateBtn(componentsDict["btn_tensao1"]) == false && panelCtrl.GetStateBtn(componentsDict["btn_tensao2"]) == false && panelCtrl.GetStateBtn(componentsDict["btn_tensao3"]) == false)
                {
                    StartCoroutine(lightGreen());
                    countBtn = 0;
                    restoreMaterial("btn_tensao1");
                    restoreMaterial("btn_tensao2");
                    restoreMaterial("btn_tensao3");
                }
                else
                {
                    StartCoroutine(lightRed());
                }
            }
        }


    }

    public void restoreMaterial(string obj)
    {
        GameObject.Find(obj).GetComponent<Renderer>().material = bkpMaterial;
    }

    public void setRedOutline(GameObject obj)
    {
    }

    IEnumerator lightGreen()
	{
        yield return new WaitForSeconds(2f);

        Material[] materiais = GameObject.Find("obj_painel_traseira").GetComponent<Renderer>().materials;

        Color bkpColor = materiais[2].color;

        Material mat = GameObject.Find("obj_painel_traseira").GetComponent<Renderer>().material;

        float emission = Mathf.PingPong(Time.time, 1.0f);

        Color baseColor = Color.green; //Replace this with whatever you want for your base color at emission level '1'

        //Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);


        materiais[2].SetColor("_EmissionColor", baseColor);

        yield return new WaitForSeconds(1f);

        materiais[2].SetColor("_EmissionColor", new Color(0, .3f, 0, 1));

        manutencaoTuto();

    }

	IEnumerator lightRed()
	{
        yield return new WaitForSeconds(2f);

        Material[] materiais = GameObject.Find("obj_painel_traseira").GetComponent<Renderer>().materials;

        Color bkpColor = materiais[0].color;

        Material mat = GameObject.Find ("obj_painel_traseira").GetComponent<Renderer>().material;
		
		float emission = Mathf.PingPong (Time.time, 1.0f);

        Color baseColor = Color.red; //Replace this with whatever you want for your base color at emission level '1'
		
		//Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);


        materiais[0].SetColor ("_EmissionColor", baseColor);

		yield return new WaitForSeconds(1f);

        materiais[0].SetColor("_EmissionColor", new Color(.3f, 0, 0, 1));

    }

	IEnumerator lightWarning()
	{

        while (emergency == true)
        {
            Material[] materiais = GameObject.Find("obj_painel_traseira").GetComponent<Renderer>().materials;

            Material mat = GameObject.Find("obj_painel_traseira").GetComponent<Renderer>().material;

            Color baseColorR = Color.red;
            Color baseColorG = Color.green;
            Color baseColorB = Color.blue;

            yield return new WaitForSeconds(.1f);

            materiais[0].SetColor("_EmissionColor", baseColorR);
            materiais[1].SetColor("_EmissionColor", baseColorG);
            materiais[2].SetColor("_EmissionColor", baseColorB);

            yield return new WaitForSeconds(.4f);

            materiais[0].SetColor("_EmissionColor", new Color(.3f, 0, 0, 1));
            materiais[1].SetColor("_EmissionColor", new Color(0, .3f, 0, 1));
            materiais[2].SetColor("_EmissionColor", new Color(0, 0, .3f, 1));
        }
    }

    IEnumerator enableBtn()
    {

        string objName = "btn_tensao1";

        panelCtrl.SetStateBtn(componentsDict[objName], true);

        Debug.Log(panelCtrl.GetStateBtn(componentsDict[objName]));

        GameObject obj = GameObject.Find(objName);

        obj.GetComponent<Animation>().Play();

        bkpMaterial = GameObject.Find(objName).GetComponent<Renderer>().material;

        yield return new WaitForSeconds(1f);

        obj.GetComponent<Renderer>().material = shaderRedOL;

        yield return new WaitForSeconds(8f);

        obj.GetComponent<Renderer>().material = shaderGreenOL;
    }

    IEnumerator enableAllBtn()
    {
        yield return new WaitForSeconds(7f);

        string objName = "btn_tensao1";
        string objName2 = "btn_tensao2";
        string objName3 = "btn_tensao3";

        panelCtrl.SetStateBtn(componentsDict[objName], true);
        panelCtrl.SetStateBtn(componentsDict[objName2], true);
        panelCtrl.SetStateBtn(componentsDict[objName3], true);

        GameObject obj = GameObject.Find(objName);
        GameObject obj2 = GameObject.Find(objName2);
        GameObject obj3 = GameObject.Find(objName3);

        obj.GetComponent<Animation>().Stop();
        obj.GetComponent<Animation>()["p_rele_do_tap1"].time = 0;
        obj.GetComponent<Animation>().Play("p_rele_do_tap1");
        obj2.GetComponent<Animation>().Play();
        obj3.GetComponent<Animation>().Play();

        bkpMaterial = GameObject.Find(objName).GetComponent<Renderer>().material;

        yield return new WaitForSeconds(1f);

        obj.GetComponent<Renderer>().material = shaderRedOL;
        obj2.GetComponent<Renderer>().material = shaderRedOL;
        obj3.GetComponent<Renderer>().material = shaderRedOL;

        yield return new WaitForSeconds(8f);

        obj.GetComponent<Renderer>().material = shaderGreenOL;
        obj2.GetComponent<Renderer>().material = shaderGreenOL;
        obj3.GetComponent<Renderer>().material = shaderGreenOL;
    }

    public void performFocus(string objName)
    {
        StartCoroutine(focusObj(objName));
    }

    public IEnumerator focusObj(string objName)
    {

        GameObject obj = GameObject.Find(objName);

        bkpMaterial2 = GameObject.Find(objName).GetComponent<Renderer>().material;

        yield return new WaitForSeconds(0f);

        obj.GetComponent<Renderer>().material = shaderGreenOL;

        yield return new WaitForSeconds(2f);

        obj.GetComponent<Renderer>().material = bkpMaterial2;

        if (tutoId == 12)
        {
            focusObj("obj_btn_vol3");
        }
        if (tutoId == 13)
        {
            focusObj("btn_disjuntor4");
        }
        if (tutoId == 14)
        {
            focusObj("btn_tensao1");
        }
        if (tutoId == 15)
        {
            focusObj("obj_btn_vol1");
        }
        if (tutoId == 16)
        {
            focusObj("btn_disjuntor2");
            focusObj("btn_disjuntor3");
        }
        if (tutoId == 17)
        {
            focusObj("btn_urgencia");
        }
        if (tutoId == 18)
        {
            focusObj("btn_tensao1");
            focusObj("btn_tensao2");
            focusObj("btn_tensao3");
        }
    }

    




    }
