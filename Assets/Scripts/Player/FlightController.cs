using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class FlightController : MonoBehaviour {

	float smooth = 5.0f;
	float angle = 30.0f;
	Vector3 spot;

	// Use this for initialization
	void Start () {
		spot = transform.localPosition;
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
		transform.rotation = Quaternion.Slerp (transform.rotation, target, Time.deltaTime * smooth);
		//Move around the screen with inputs, bounded at the edges
		spot.x += (h/10);
		spot.y += (v/10);
		if (spot.x > 4f) 
		{
			spot.x = 4f;
		}
		if (spot.x < -4f) 
		{
			spot.x = -4f;
		}
		if (spot.y > 1.2f) 
		{
			spot.y = 1.2f;
		}
		if (spot.y < -1.2f) 
		{
			spot.y = -1.2f;
		}
		transform.localPosition = spot;
	}
}
