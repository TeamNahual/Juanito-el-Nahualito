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

	public PhysicMaterial physMaterial;

	public GameObject capsuleCol;

	Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();

		colliders = transform.Find("Colliders").gameObject.GetComponents<Collider>();

		foreach(Collider col in colliders)
		{
			col.material = physMaterial;
		}

		allObjects = new GameObject[1 + transform.childCount];

		allObjects[0] = gameObject;

		for(int i = 0; i < transform.childCount; i++)
		{
			allObjects[i + 1] = transform.GetChild(i).gameObject;
		}

		if(capsuleCol != null)
		{
			Physics.IgnoreCollision(capsuleCol.GetComponent<Collider>(), Juanito.ins.JuanitoSpirit.GetComponent<Collider>(), true);
		}

		for(int i = 0; i < colliders.Length; i++)
		{
			Physics.IgnoreCollision(colliders[i], Juanito.ins.JuanitoHuman.GetComponent<Collider>(), true);
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

			Vector3 camera_forward = Vector3.Scale (Camera.main.transform.forward, new Vector3 (1, 0, 1)).normalized;

			// Debug.Log(Juanito.ins.JuanitoSpirit.transform.forward * vertical);
			rb.velocity = ((vertical * camera_forward) + 
						(horizontal * Camera.main.transform.right)) * speed;
			// Juanito.ins.JuanitoSpirit.transform.localPosition = initialPosition;

			Vector3 diff = Juanito.ins.JuanitoSpirit.transform.position - transform.position;
			diff = new Vector3 (diff.x, 0f, diff.z);
			float animForward = vertical;
			float angleDiff = Vector3.Angle (camera_forward, diff);
			if (angleDiff < 90f) {
				animForward *= -1;
			}

			Juanito.ins.SpiritAnim.SetFloat("Forward", animForward);
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
		Juanito.ins.SpiritAnim.SetBool("Pushing", true);
		//rb.detectCollisions = true;
		pushing = true;
		ToggleColliders(true);
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		Juanito.ins.JuanitoSpirit.transform.parent = transform;
		Juanito.ins.JuanitoSpirit.GetComponent<ThirdPersonUserControl>().enabled = false;
 		Juanito.ins.JuanitoSpirit.GetComponent<ThirdPersonCharacter>().enabled = false;
 		Juanito.ins.JuanitoSpirit.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
 		initialPosition = Juanito.ins.JuanitoSpirit.transform.localPosition;

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
		Juanito.ins.SpiritAnim.SetBool("Pushing", false);
		//rb.detectCollisions = false;
		pushing = false;
		ToggleColliders(false);
		rb.constraints = RigidbodyConstraints.FreezeAll;
		UIManager.instance.TooltipDisable();
		Juanito.ins.JuanitoSpirit.transform.parent = Juanito.ins.transform;
		Juanito.ins.JuanitoSpirit.GetComponent<ThirdPersonUserControl>().enabled = true;
 		Juanito.ins.JuanitoSpirit.GetComponent<ThirdPersonCharacter>().enabled = true;
 		Juanito.ins.JuanitoSpirit.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
 		
 		if(capsuleCol != null)
 		{	
 			capsuleCol.SetActive(false);
 		}

	}

	void ToggleColliders(bool ignore)
	{
		for(int i = 0; i < colliders.Length; i++)
		{
			Physics.IgnoreCollision(colliders[i], Juanito.ins.JuanitoSpirit.GetComponent<Collider>(), ignore);
		}
	}
}

