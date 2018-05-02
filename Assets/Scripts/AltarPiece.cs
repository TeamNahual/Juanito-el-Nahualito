using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AltarPiece : MonoBehaviour {

	public bool SeashellRelic;
	public bool TorusRelic;
	public bool NecklaceRelic;

	public Altar main;

	// Use this for initialization
	void Start () {
		main = transform.parent.gameObject.GetComponent<Altar>();

		transform.GetChild(0).gameObject.SetActive(false);
	}
	
	void DisablePiece()
	{
		UIManager.instance.TooltipDisable();
		GetComponent<Collider>().enabled = false;
	}

	// Update is called once per frame
	void Update () {

	}

	void HandleSeashell()
	{
		if(Juanito.ins.statueRelic)
		{
			if(!main.SeashellRelic)
			{
				UIManager.instance.TooltipInteract();

				if (Input.GetKeyDown (KeyCode.E) || CrossPlatformInputManager.GetButtonDown ("Action")) 
				{
					DisablePiece();
					transform.GetChild (0).gameObject.SetActive (true);
					main.SeashellRelic = true;
				}
			}
			else
			{
				UIManager.instance.TooltipDisable();
				GetComponent<Collider>().enabled = false;
			}
		}
	}

	void HandleTorus()
	{
		if(Juanito.ins.butterflyRelic)
		{
			if(!main.TorusRelic)
			{
				UIManager.instance.TooltipInteract();

				if (Input.GetKeyDown (KeyCode.E) || CrossPlatformInputManager.GetButtonDown ("Action")) 
				{
					DisablePiece();
					transform.GetChild (0).gameObject.SetActive (true);
					main.TorusRelic = true;
				}
			}
			else
			{
				UIManager.instance.TooltipDisable();
				GetComponent<Collider>().enabled = false;
			}
		}
	}

	void HandleNecklace()
	{
		if(main.TorusRelic && main.SeashellRelic && !main.NecklaceRelic)
		{
			UIManager.instance.TooltipInteract();

			if (Input.GetKeyDown (KeyCode.E) || CrossPlatformInputManager.GetButtonDown ("Action")) 
			{
				DisablePiece();
				transform.GetChild (0).gameObject.SetActive (true);
				main.NecklaceRelic = true;

				UIManager.instance.dialogueSystem.addDialogue("To be continued....");
			}
		}
		else
		{
			UIManager.instance.TooltipDisable();
		}
	}


	void OnTriggerStay(Collider other)
	{
		if(other.gameObject == Juanito.ins.JuanitoHuman)
		{
			if(Juanito.ins.CheckFacingObjects(new GameObject[] {transform.parent.gameObject, gameObject}))
			{
				if(SeashellRelic)
				{
					HandleSeashell();
				}
				else if(TorusRelic)
				{
					HandleTorus();
				}
				else if(NecklaceRelic)
				{
					HandleNecklace();
				}
			}
			else
			{
				UIManager.instance.TooltipDisable();
			}
		}
	}
}
