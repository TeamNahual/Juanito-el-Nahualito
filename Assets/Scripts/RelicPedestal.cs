using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class RelicPedestal : MonoBehaviour {

	public Vector3 finalPosition;
	public Vector3 beginningPosistion;

	public bool active = true;

	// Use this for initialization
	void Start () {
		finalPosition = new Vector3 (transform.localPosition.x, 2.457f, transform.localPosition.z);
		beginningPosistion = new Vector3 (transform.localPosition.x, 2f, transform.localPosition.z);

		transform.localPosition = beginningPosistion;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject == Juanito.ins.JuanitoHuman && active) 
		{
			if (Input.GetKeyDown (KeyCode.E) || CrossPlatformInputManager.GetButtonDown ("Action")) 
			{
				transform.GetChild (0).gameObject.SetActive (false);
				Juanito.ins.butterflyRelic = true;
				UIManager.instance.dialogueSystem.addDialogue("You picked up a relic.");
				active = false;
			}		
		}
	}


}
