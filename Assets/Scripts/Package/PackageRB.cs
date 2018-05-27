using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class PackageRB : MonoBehaviour {

	public float speed = 5.0f;
	public bool active;
	public bool pushing;

	public Vector3 initialPosition;

	public GameObject capsuleCol;

	public Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		if(capsuleCol != null)
		{
			Physics.IgnoreCollision(capsuleCol.GetComponent<Collider>(), Juanito.ins.JuanitoHuman.GetComponent<Collider>(), true);
			Physics.IgnoreCollision(GetComponent<Collider>(), capsuleCol.GetComponent<Collider>());
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!active) {
			pushing = false;
			Juanito.ins.isPushing = false;
		} else
			Juanito.ins.isPushing = false;

		if(pushing)
		{
			//Added to Use in another Sound Script
			//Juanito.ins.isPushing = true;

			float horizontal = GetPlayerInput()[0];
			float vertical = GetPlayerInput()[1];

			if (horizontal > 0 || horizontal < 0 || vertical > 0 || vertical < 0) {
				Juanito.ins.isPushing = true;
			}// else
				//Juanito.ins.isPushing = false;

			Vector3 camera_forward = Vector3.Scale (Camera.main.transform.forward, new Vector3 (1, 0, 1)).normalized;

			// Debug.Log(Juanito.ins.JuanitoHuman.transform.forward * vertical);
			Vector3 velocity = ((vertical * camera_forward) + 
						(horizontal * Camera.main.transform.right)) * speed;

			velocity.y = rb.velocity.y;

			rb.velocity = velocity;
			// Juanito.ins.JuanitoHuman.transform.localPosition = initialPosition;

			Vector3 diff = Juanito.ins.JuanitoHuman.transform.position - transform.position;
			diff = new Vector3 (diff.x, 0f, diff.z);
			float animForward = vertical;
			float angleDiff = Vector3.Angle (camera_forward, diff);
			if (angleDiff < 90f) {
				animForward *= -1;
			}

			Juanito.ins.HumanAnim.SetFloat("Forward", animForward);
		}
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
		pushing = true;
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		Juanito.ins.JuanitoHuman.transform.parent = transform;
		Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = false;
 		Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = false;
 		Juanito.ins.JuanitoHuman.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
 		initialPosition = Juanito.ins.JuanitoHuman.transform.localPosition;

 		if(capsuleCol != null)
 		{	
 			capsuleCol.SetActive(true);
 			capsuleCol.transform.localPosition = new Vector3(
 				initialPosition.x,
 				capsuleCol.transform.localPosition.y,
 				initialPosition.z);
 		}
	}

	public void DetachPlayer()
	{
		Juanito.ins.HumanAnim.SetBool("Pushing", false);
		//rb.detectCollisions = false;
		pushing = false;
		rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		UIManager.instance.TooltipDisable();
		Juanito.ins.JuanitoHuman.transform.parent = Juanito.ins.transform;
		Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = true;
 		Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = true;
 		Juanito.ins.JuanitoHuman.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
 		
 		if(capsuleCol != null)
 		{	
 			capsuleCol.SetActive(false);
 		}

	}

	public void Activate(){
		Debug.Log ("package activate");
		active = true;
		rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
	}

	public void Deactivate(){
		Debug.Log ("package deactivate");
		active = false;
		DetachPlayer ();
		rb.constraints = RigidbodyConstraints.FreezeAll;
	}
}
