using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class FlightController : MonoBehaviour {

	float smooth = 5.0f;
	float angle = 45.0f;
	float fullTurn = 180f;
	float addingDegree = 0f;
	Vector3 spot;
	Animator controller;
	public Vector3 birdX;
	private Vector3 localPos;

	public Rigidbody birdRigidBody;
	public float birdFwrd;
	public float birdHSpeed;
	private float maxYRotation;
	private float minYRotation;
	private float yRotation;

	// Use this for initialization
	void Start () {
		spot = transform.localPosition;
		controller = GetComponentInChildren<Animator> ();

		birdRigidBody = GetComponent<Rigidbody> ();
		birdFwrd = 50.0f;
		birdHSpeed = 20.0f;

		//Rotation Values
		maxYRotation = 0.4f;
		minYRotation = 0.4f;

		localPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
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
		Quaternion target = Quaternion.Euler(-v*angle, addingDegree,-h*angle);
		//Quaternion ySave = Quaternion.Euler (v*angle, 0, h*angle);
		//yRotation = transform.localRotation.y;
		//yRotation = h * fullTurn;
		Debug.Log ("Quaternion Rotation: " + target);
		//Return to resting rotation when not pressing buttons
		transform.localRotation = Quaternion.Slerp (transform.localRotation, target, Time.deltaTime * smooth);

		//Move around the screen with inputs, bounded at the edges
		//spot.x += (h/3);
		//spot.y += (v/10);

		//automatically move forward
		birdRigidBody.velocity = transform.forward * birdFwrd;

		//Debug.Log ("Rigid Body: " + birdRigidBody.velocity);
		//Move to the sides
		if (h > 0) {
			//birdRigidBody.velocity = transform.right * birdHSpeed;
			//birdRigidBody.velocity = new Vector3 (birdHSpeed, 0f, birdFwrd);
			birdRigidBody.velocity = (transform.right * birdHSpeed) + (transform.forward * birdFwrd);
			yRotation = h * fullTurn;

			// Turns bird in Quaternion target
			addingDegree += 0.75f;
		}
		if (h < 0) {
			//birdRigidBody.AddForce(transform.right * -birdHSpeed);
			//birdRigidBody.velocity = new Vector3 (-birdHSpeed, 0f, birdFwrd);
			birdRigidBody.velocity = (transform.right * -birdHSpeed) + (transform.forward * birdFwrd);

			// Turns bird in Quaternion target
			addingDegree -= 0.75f;
		}

		//gameObject.transform.forward
		/*if (spot.x > 8f) 
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
		}*/
		localPos.x = spot.x;
		localPos.y = 0;
		//localPos.z = speed; 
		//transform.localPosition = localPos;
		if (v == 0) 
		{
			v = -1;
		}
		controller.SetFloat ("Vertical", v);
	}
}
