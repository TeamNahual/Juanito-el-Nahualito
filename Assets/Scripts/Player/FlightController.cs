using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class FlightController : MonoBehaviour {

	float smooth = 5.0f;
	float angle = 45.0f;
	Vector3 spot;
	Animator controller;

	// Use this for initialization
	void Start () {
		spot = transform.localPosition;
		controller = GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");
		float s;
		if (Input.GetKey (KeyCode.E)) 
		{
			s = .4f;
		} 
		else if (Input.GetKey (KeyCode.Q)) 
		{
			s = -.4f;
		} 
		else 
		{
			s = 0f;
		}
		bool keyboard = !(Mathf.Abs(h) < 0.1f && Mathf.Abs(v) < 0.1f);

		if (!keyboard)
		{
			h = CrossPlatformInputManager.GetAxis("Horizontal-Joystick");
			v = CrossPlatformInputManager.GetAxis("Vertical-Joystick");
			s = CrossPlatformInputManager.GetAxis ("Vertical-Joystick-Right");
		}

		//Applying inputs to rotations
		Quaternion target = Quaternion.Euler(-v*angle,0,-h*angle);
		//Return to resting rotation when not pressing buttons
		transform.localRotation = Quaternion.Slerp (transform.localRotation, target, Time.deltaTime * smooth);
		//Move around the screen with inputs, bounded at the edges
		spot.x += (h/10);
		spot.y += (v/10);
		if (spot.x > 8f) 
		{
			spot.x = 8f;
			Debug.Log ("right");
		}
		if (spot.x < -8f) 
		{
			spot.x = -8f;
			Debug.Log ("left");
		}
		if (spot.y > 3f) 
		{
			spot.y = 3f;
			Debug.Log ("up");
		}
		if (spot.y < -4f) 
		{
			spot.y = -4f;
			Debug.Log ("down");
		}
		transform.localPosition = spot;
		if (v == 0) 
		{
			v = -1;
		}
		controller.SetFloat ("Vertical", v);
	}
}
