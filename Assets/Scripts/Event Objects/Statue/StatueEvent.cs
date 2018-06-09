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
		if(other.gameObject != Juanito.ins.JuanitoSpirit)
			return;

		if(container.disabled)
		{
			if(pushing)
			{
				Dettach();
 				//UIManager.instance.dialogueSystem.addDialogue("The statue locks in place and refuses to budge.");
 				StatuePuzzleManager.ins.CheckStatueRotations();

				//Stop playing sound effect
				rotatingStones.Stop ();
			}

			return;
		}

		Vector3 fwd = Juanito.ins.JuanitoSpirit.transform.TransformDirection(Vector3.forward);
        Vector3 dir = Vector3.Normalize(transform.position - Juanito.ins.JuanitoSpirit.transform.position);
		bool dirCheck = Vector3.Dot(fwd, dir) > 0.4;

		if(dirCheck)
		{
			UIManager.instance.TooltipDisplay("Hold <sprite=0> to Interact");

			if((Input.GetKey(KeyCode.E) || CrossPlatformInputManager.GetButton("Action")))
			{
				UIManager.instance.TooltipDisplay("Use <sprite=6> to Push/Pull");
				
				if(!pushing)
				{
					Attach();
				}
			}
			else
			{
				if(pushing)
				{
					UIManager.instance.TooltipDisable();
					Dettach();
				}
			}
		}
		else
		{
			UIManager.instance.TooltipDisable();
		}

		if(pushing)
		{
            HandlePush();
		}
	}

	void HandlePush()
	{
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
		bool keyboard = !(Mathf.Abs(h) < 0.1f && Mathf.Abs(v) < 0.1f);

		// Debug.Log("Pushing " + keyboard);

		if (!keyboard)
		{
			h = CrossPlatformInputManager.GetAxis("Horizontal-Joystick");
			v = CrossPlatformInputManager.GetAxis("Vertical-Joystick");
		}

		Juanito.ins.SpiritAnim.SetFloat("Forward", v);

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
	}

	void Attach()
	{
		Juanito.ins.SpiritAnim.SetBool("Pushing", true);
		pushing = true;
		Juanito.ins.JuanitoSpirit.transform.parent = transform.parent;
		Juanito.ins.JuanitoSpirit.GetComponent<ThirdPersonUserControl>().enabled = false;
		Juanito.ins.JuanitoSpirit.GetComponent<ThirdPersonCharacter>().enabled = false;
	}

	void Dettach()
	{
		Juanito.ins.SpiritAnim.SetBool("Pushing", false);
		pushing = false;
		Juanito.ins.JuanitoSpirit.transform.parent = Juanito.ins.transform;
		Juanito.ins.JuanitoSpirit.GetComponent<ThirdPersonUserControl>().enabled = true;
		Juanito.ins.JuanitoSpirit.GetComponent<ThirdPersonCharacter>().enabled = true;
		UIManager.instance.TooltipDisable();
		// Stop playing sound
		rotatingStones.Stop();
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject == Juanito.ins.JuanitoSpirit)
		{
			if(pushing)
			{
				Dettach();
			}
		}
		if (rotatingStones.isPlaying) {
			rotatingStones.Stop ();
		}
	}
}
