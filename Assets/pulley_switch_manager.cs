using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class pulley_switch_manager : MonoBehaviour {

	private pulley_manager manager;
	private bool overlap = false;
	private Animator animator;
	private bool problemFlag = false;

	void Awake(){
		manager = GetComponentInParent <pulley_manager> ();
		animator = GetComponentInParent <Animator> ();
	}
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject == Juanito.ins.JuanitoHuman)
			overlap = true;
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject == Juanito.ins.JuanitoHuman) {
			overlap = false;
			problemFlag = false;
		}
	}

	void Update(){
		if (overlap) 
		{
			Vector3 fwd = Juanito.ins.JuanitoHuman.transform.TransformDirection(Vector3.forward);
	        Vector3 dir = Vector3.Normalize(transform.position - Juanito.ins.JuanitoHuman.transform.position);
			bool dirCheck = Vector3.Dot(fwd, dir) > 0.5;

			if (dirCheck && !problemFlag)//if player is facing the person, turn on the tool tip.
	        {
	            UIManager.instance.TooltipDisplay("Press <sprite=0> to use pulley"); //displays message saying press X to talk
	        }
	        if (!dirCheck) //if player is not facing the person, turn off the tool tip.
	        {
	            UIManager.instance.TooltipDisable(); //turns off the tooltips
	            UIManager.instance.toggleDialogueBox(false);
	        }

			if(Input.GetKeyDown (KeyCode.E) || CrossPlatformInputManager.GetButtonDown("Action"))
			{
				Debug.Log ("Switch hit");

				if (manager.MovePlatforms ()) {
					animator.SetBool ("pulled", true);
				} else {
					UIManager.instance.TooltipDisplay ("The pulley must be loaded before it can be used");
					problemFlag = true;
				}
			}
		}
	}
}
