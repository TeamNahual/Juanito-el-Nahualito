using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class ButterflyPiece : MonoBehaviour {

	public bool isPushing = false;
	public int directionFlag;
	public Vector3 movementVector;
	public Vector3 sideMovementVector;
	public bool limitForward, limitBackward, limitLeft, limitRight = false;
	public Rigidbody rb;
	public bool locked;

	public GameObject[] meshes;

	public float pushSpeed = 0.01f;

	// Use this for initialization
	void Start () {
		//rb.detectCollisions = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(isPushing && !locked)
			PushHandler();
	}

	void PushHandler()
	{
		float[] playerInput = GetPlayerInput();

		float hInput = playerInput [0];
		float vInput = playerInput [1];

		if (limitRight && hInput > 0)
			hInput = 0;
		else if (limitLeft && hInput < 0)
			hInput = 0;

		if (limitForward && vInput > 0)
			vInput = 0;
		else if (limitBackward && vInput < 0)
			vInput = 0;

		Juanito.ins.HumanAnim.SetFloat("Forward", vInput);

		transform.Translate(vInput * movementVector * directionFlag * pushSpeed);
		transform.Translate(hInput * sideMovementVector * directionFlag * pushSpeed);
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
		Juanito.ins.HumanAnim.SetBool("Pushing", true);
		//rb.detectCollisions = true;
		Juanito.ins.transform.parent = transform;
		Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = false;
 		Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = false;
	}

	public void DetachPlayer()
	{
		Juanito.ins.HumanAnim.SetBool("Pushing", false);
		//rb.detectCollisions = false;
		UIManager.instance.TooltipDisable();
		Juanito.ins.transform.parent = null;
		Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = true;
 		Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = true;
	}
}
