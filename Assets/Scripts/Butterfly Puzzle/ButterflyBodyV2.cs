using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.ThirdPerson;

public class ButterflyBodyV2 : MonoBehaviour {

	public float speed = 5.0f;
	public bool collidingJuanito = false;

	public bool pushing = false;

	public LayerMask layerMask;

	public GameObject[] allObjects;

	public Vector3 initialPosition;

	Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();

	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject == Juanito.ins.JuanitoSpirit)
		{
			// Debug.Log("Juanito Enter");
			collidingJuanito = true;
		}
	}

	void OnCollisionExit(Collision other)
	{
		if(other.gameObject == Juanito.ins.JuanitoSpirit)
		{
			// Debug.Log("Juanito Leave");
			collidingJuanito = false;
		}
	}

	void FixedUpdate()
	{
		if(pushing)
			collidingJuanito = true;

		if(true)//Juanito.ins.CheckFacingObjectsSpirit(allObjects, layerMask))
		{
			UIManager.instance.TooltipDisplay("Hold <sprite=0> to Interact");


			if(Input.GetKey(KeyCode.E) || CrossPlatformInputManager.GetButton("Action"))
			{
				if(!pushing)
					AttachPlayer();
			}
			else
			{
				if(pushing)
					DetachPlayer();
			}

			if(pushing)
			{
				float horizontal = GetPlayerInput()[0];
				float vertical = GetPlayerInput()[1];

				Juanito.ins.JuanitoSpirit.transform.localPosition = initialPosition;
				// Debug.Log(Juanito.ins.JuanitoSpirit.transform.forward * vertical);
				rb.velocity = ((Juanito.ins.JuanitoSpirit.transform.forward * vertical) + 
							(Juanito.ins.JuanitoSpirit.transform.right * horizontal)) * speed;
			}
		}
		else
		{
			UIManager.instance.TooltipDisable();
			DetachPlayer();
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
		// Juanito.ins.HumanAnim.SetBool("Pushing", true);
		//rb.detectCollisions = true;
		pushing = true;
		Physics.IgnoreCollision(GetComponentInChildren<Collider>(), Juanito.ins.JuanitoSpirit.GetComponent<Collider>());
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		Juanito.ins.JuanitoSpirit.transform.parent = transform;
		Juanito.ins.JuanitoSpirit.GetComponent<ThirdPersonUserControl>().enabled = false;
 		Juanito.ins.JuanitoSpirit.GetComponent<ThirdPersonCharacter>().enabled = false;
 		initialPosition = Juanito.ins.JuanitoSpirit.transform.localPosition;
	}

	public void DetachPlayer()
	{
		// Juanito.ins.HumanAnim.SetBool("Pushing", false);
		//rb.detectCollisions = false;
		pushing = false;
		Physics.IgnoreCollision(GetComponentInChildren<Collider>(), Juanito.ins.JuanitoSpirit.GetComponent<Collider>(), false);
		rb.constraints = RigidbodyConstraints.FreezeAll;
		UIManager.instance.TooltipDisable();
		Juanito.ins.JuanitoSpirit.transform.parent = Juanito.ins.transform;
		Juanito.ins.JuanitoSpirit.GetComponent<ThirdPersonUserControl>().enabled = true;
 		Juanito.ins.JuanitoSpirit.GetComponent<ThirdPersonCharacter>().enabled = true;
	}
}

