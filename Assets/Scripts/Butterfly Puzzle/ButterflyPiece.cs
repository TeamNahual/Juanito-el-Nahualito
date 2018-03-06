using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class ButterflyPiece : MonoBehaviour {

	public bool isPushing = false;
	public int directionFlag;
	public Vector3 movementVector;
	public bool limitForward, limitBackward = false;
	public Rigidbody rb;
	public bool locked;

	public float pushSpeed = 0.01f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(isPushing && !locked)
			PushHandler();
	}

	void PushHandler()
	{
		float[] playerInput = GetPlayerInput();

		float vInput = playerInput [1];

		if (limitForward && vInput > 0)
			vInput = 0;
		else if (limitBackward && vInput < 0)
			vInput = 0;

		transform.Translate(vInput * movementVector * directionFlag * pushSpeed);
	}

	public float[] GetPlayerInput()
	{
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
		bool keyboard = !(Mathf.Abs(h) < 0.1f && Mathf.Abs(v) < 0.1f);

		if (!keyboard)
		{
			h = CrossPlatformInputManager.GetAxis("Horizontal-Joystick");
			v = CrossPlatformInputManager.GetAxis("Vertical-Joystick");
		}

		return new float[] {h,v};
	}

	public void AttachPlayer()
	{
		Juanito.ins.transform.parent = transform;
		Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = false;
 		Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = false;
	}

	public void DetachPlayer()
	{
		Juanito.ins.transform.parent = null;
		Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = true;
 		Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = true;
	}
}
