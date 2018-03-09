using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class package : MonoBehaviour {

	public float pushSpeed;

	private Rigidbody rb;
	private bool pushing = false;
	private float horizontal = 0, vertical = 0;
	private bool collidingJuanito = false;

	// Use this for initialization
	void Start () {
		rb = GetComponent <Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.E)) {
			print ("key down");
			if (!pushing && collidingJuanito) {
				print ("Pushing");
				pushing = true;
				Juanito.ins.transform.parent = this.transform;
				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = false;
				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = false;
			} else if(pushing) {
				print ("Letting go");
				pushing = false;
				Juanito.ins.transform.parent = null;
				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = true;
				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = true;
			}
		}

		if(pushing)
		{
			horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
			vertical = CrossPlatformInputManager.GetAxis("Vertical");
			bool keyboard = !(Mathf.Abs(horizontal) < 0.1f && Mathf.Abs(vertical) < 0.1f);

			if (!keyboard)
			{
				horizontal = CrossPlatformInputManager.GetAxis("Horizontal-Joystick");
				vertical = CrossPlatformInputManager.GetAxis("Vertical-Joystick");
			}

			//These dimensions might need to change later
			if (Physics.Linecast(
				transform.position + new Vector3(horizontal, 0f, vertical),
				transform.position + new Vector3(horizontal, -0.6f, vertical)
			)) {
				rb.velocity = new Vector3 (horizontal, 0f, vertical).normalized * pushSpeed;
			}
		}
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject != Juanito.ins.JuanitoHuman) {
				
		}
	}

	void OnCollisionExit(Collision other){
		if (other.gameObject != Juanito.ins.JuanitoHuman) {
			
		}
	}

	void OnCollisionStay(Collision other){
		collidingJuanito = (other.gameObject == Juanito.ins.JuanitoHuman);
	}
}
