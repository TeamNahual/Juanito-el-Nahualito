using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class RelicPedestal : MonoBehaviour {

	public GameObject[] targetObjects;

	public Vector3 finalPosition;
	public Vector3 beginningPosistion;

	public bool ButterflyRelic;
	public bool StatueRelic;

	public bool active = true;

	// Use this for initialization
	void Start () {
		finalPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
		beginningPosistion = new Vector3 (transform.localPosition.x, transform.localPosition.y - 0.457f, transform.localPosition.z);

		transform.localPosition = beginningPosistion;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject == Juanito.ins.JuanitoHuman && active && Juanito.ins.CheckFacingObjects(targetObjects)) 
		{

			UIManager.instance.pushHelp.SetActive(true);

			if (Input.GetKeyDown (KeyCode.E) || CrossPlatformInputManager.GetButtonDown ("Action")) 
			{
				UIManager.instance.pushHelp.SetActive(false);
				transform.GetChild (0).gameObject.SetActive (false);
				if(ButterflyRelic) Juanito.ins.butterflyRelic = true;
				if(StatueRelic) Juanito.ins.statueRelic = true;
				UIManager.instance.dialogueSystem.addDialogue("You picked up a relic.");
				active = false;
			}		
		}
		else
		{
			UIManager.instance.pushHelp.SetActive(false);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == Juanito.ins.JuanitoHuman)
		{
			UIManager.instance.pushHelp.SetActive(false);
		}
	}


}
