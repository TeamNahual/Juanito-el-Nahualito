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
	private Transform camera;
	private Vector3 camera_forward;

	void Awake(){
		rb = GetComponent <Rigidbody> ();
		camera = Camera.main.transform;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.E)) 
		{
			print ("key down");
			if (collidingJuanito) 
			{
				print ("Pushing");
				pushing = true;
				Juanito.ins.transform.parent = this.transform;
				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = false;
				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = false;
			}
		} 
		else
		{
			if(pushing) 
			{
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
			float hBuffer;
			if (horizontal > 0) {
				hBuffer = 0.1f;
			} else if (horizontal < 0) {
				hBuffer = -0.1f;
			} else {
				hBuffer = 0f;
			}
			float vBuffer;
			if (vertical> 0) {
				vBuffer = 0.1f;
			} else if (vertical < 0) {
				vBuffer = -0.1f;
			} else {
				vBuffer = 0f;
			}
			if (Physics.Linecast(
				transform.position + new Vector3(horizontal, -0.4f, vertical),
				transform.position + new Vector3(horizontal, -0.6f, vertical))
				||
				Physics.Linecast(
					transform.position + new Vector3(horizontal+hBuffer, -0.4f, vertical+vBuffer),
					transform.position + new Vector3(horizontal+hBuffer, -0.6f, vertical+vBuffer))
			) {
				print ("linecast hit");
				camera_forward = Vector3.Scale(camera.forward, new Vector3(1, 0, 1)).normalized;
				Vector3 move = vertical * camera_forward + horizontal * camera.right;
				rb.velocity = move * pushSpeed;
			}
			Debug.DrawLine (
				transform.position + new Vector3(horizontal, -0.4f, vertical),
				transform.position + new Vector3(horizontal, -0.6f, vertical),
				Color.red
			);
			Debug.DrawLine (
				transform.position + new Vector3(horizontal+hBuffer, -0.4f, vertical+vBuffer),
				transform.position + new Vector3(horizontal+hBuffer, -0.6f, vertical+vBuffer),
				Color.red
			);
		}
	}

	void OnCollisionStay(Collision other){
		collidingJuanito = (other.gameObject == Juanito.ins.JuanitoHuman);
	}
}