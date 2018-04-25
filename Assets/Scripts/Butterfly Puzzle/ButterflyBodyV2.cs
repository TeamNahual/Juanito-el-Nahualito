using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.ThirdPerson;

public class ButterflyBodyV2 : MonoBehaviour {

	public float speed = 5.0f;

	public bool pushing = false;

	public bool locked = false;

	public LayerMask layerMask;

	public GameObject[] allObjects;

	public Vector3 initialPosition;

	public Collider[] colliders;

	Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();

		colliders = transform.Find("Colliders").gameObject.GetComponents<Collider>();

		allObjects = new GameObject[1 + transform.childCount];

		allObjects[0] = gameObject;

		for(int i = 0; i < transform.childCount; i++)
		{
			allObjects[i + 1] = transform.GetChild(i).gameObject;
		}
	}

	void FixedUpdate()
	{
		if(locked)
			pushing = false;

		if(pushing)
		{
			float horizontal = GetPlayerInput()[0];
			float vertical = GetPlayerInput()[1];
			// Debug.Log(Juanito.ins.JuanitoSpirit.transform.forward * vertical);
			rb.velocity = ((Juanito.ins.JuanitoSpirit.transform.forward * vertical) + 
						(Juanito.ins.JuanitoSpirit.transform.right * horizontal)) * speed;
			// Juanito.ins.JuanitoSpirit.transform.localPosition = initialPosition;
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
		ToggleColliders(true);
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		Juanito.ins.JuanitoSpirit.transform.parent = transform;
		Juanito.ins.JuanitoSpirit.GetComponent<ThirdPersonUserControl>().enabled = false;
 		Juanito.ins.JuanitoSpirit.GetComponent<ThirdPersonCharacter>().enabled = false;
 		Juanito.ins.JuanitoSpirit.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
 		initialPosition = Juanito.ins.JuanitoSpirit.transform.localPosition;
	}

	public void DetachPlayer()
	{
		// Juanito.ins.HumanAnim.SetBool("Pushing", false);
		//rb.detectCollisions = false;
		pushing = false;
		ToggleColliders(false);
		rb.constraints = RigidbodyConstraints.FreezeAll;
		UIManager.instance.TooltipDisable();
		Juanito.ins.JuanitoSpirit.transform.parent = Juanito.ins.transform;
		Juanito.ins.JuanitoSpirit.GetComponent<ThirdPersonUserControl>().enabled = true;
 		Juanito.ins.JuanitoSpirit.GetComponent<ThirdPersonCharacter>().enabled = true;
 		Juanito.ins.JuanitoSpirit.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

	}

	void ToggleColliders(bool ignore)
	{
		for(int i = 0; i < colliders.Length; i++)
		{
			Physics.IgnoreCollision(colliders[i], Juanito.ins.JuanitoSpirit.GetComponent<Collider>(), ignore);
		}
	}
}

