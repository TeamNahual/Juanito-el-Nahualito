using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Audio;


public class StatueEvent : EventObject {

	public bool pushing = false;
	public int rotateFlag = 1;

	public StatueContainer container; 

	public AudioSource rotatingStones;

	float push_strength = 0.5f;

	void Start(){
		rotatingStones = gameObject.GetComponent<AudioSource> ();
	}

	void OnTriggerStay(Collider other) {
		if(other.gameObject != Juanito.ins.JuanitoHuman)
			return;

		if(container.disabled)
		{
			if(pushing)
			{
				pushing = false;
				UIManager.instance.TooltipDisable();
				Juanito.ins.HumanAnim.SetBool("Pushing", false);
				Juanito.ins.transform.parent = null;
				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = true;
 				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = true;
 				//UIManager.instance.dialogueSystem.addDialogue("The statue locks in place and refuses to budge.");
 				StatuePuzzleManager.ins.CheckStatueRotations();

				//Stop playing sound effect
				rotatingStones.Stop ();
			}

			return;
		}

		UIManager.instance.TooltipDisplay("Hold <sprite=0> to Interact");

		if((Input.GetKey(KeyCode.E) || CrossPlatformInputManager.GetButton("Action"))
			 && CheckPlayerDirection(Juanito.ins.JuanitoHuman))
		{
			UIManager.instance.TooltipDisplay("Use <sprite=6> to Push/Pull");
			Juanito.ins.HumanAnim.SetBool("Pushing", true);
			pushing = true;
			Juanito.ins.transform.parent = transform.parent;
			Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = false;
			Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = false;
		}
		else
		{
			if(pushing)
			{
				Juanito.ins.HumanAnim.SetBool("Pushing", false);
				UIManager.instance.TooltipDisable();
				pushing = false;
				Juanito.ins.transform.parent = null;
				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = true;
 				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = true;

				// Stop playing sound
				rotatingStones.Stop();
			}
		}

		if(pushing)
		{
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
			bool keyboard = !(Mathf.Abs(h) < 0.1f && Mathf.Abs(v) < 0.1f);

			Debug.Log("Pushing " + keyboard);

			if (!keyboard)
			{
				h = CrossPlatformInputManager.GetAxis("Horizontal-Joystick");
				v = CrossPlatformInputManager.GetAxis("Vertical-Joystick");
			}

			Juanito.ins.HumanAnim.SetFloat("Forward", v);

            transform.parent.transform.Rotate(v * Vector3.up * rotateFlag * push_strength);
            container.currentRotation += v * rotateFlag * push_strength;

			// Play Rotating Stone Sound Effect * test using controllers
			if (keyboard) {
				if (!rotatingStones.isPlaying) {
					rotatingStones.Play ();
				} 
			}
			if (!keyboard) {
				rotatingStones.Stop ();
			}
			/*if (rotatingStones.isPlaying){
				if (!Input.GetKey(KeyCode.E) || !CrossPlatformInputManager.GetButton("Action")){
					rotatingStones.Stop();
				}
			}*/
		}
	}


	void OnTriggerExit(Collider other)
	{
		if(other.gameObject == Juanito.ins.JuanitoHuman)
		{
			if(pushing)
			{
				Juanito.ins.HumanAnim.SetBool("Pushing", false);
				pushing = false;
				Juanito.ins.transform.parent = null;
				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = true;
 				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = true;

				if (rotatingStones.isPlaying) {
					rotatingStones.Stop ();
				}
			}

			UIManager.instance.TooltipDisable();
		}
		if (rotatingStones.isPlaying) {
			rotatingStones.Stop ();
		}
	}
}
