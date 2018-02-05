using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritController : MonoBehaviour {

	public GameObject currentFollower;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// InteractWithObject();
	}

	void InteractWithObject()
	{
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Ray ray = new Ray(transform.position + Vector3.up, fwd);
		RaycastHit hit;

		Debug.DrawRay(transform.position + Vector3.up, fwd, Color.green);
		if(Physics.Raycast(ray, out hit, 1))
		{
			Debug.DrawLine(ray.origin, hit.point, Color.red);
			Debug.Log(hit.transform.gameObject);

			EventObject obj = hit.transform.gameObject.GetComponent<EventObject>();

			if(obj)
			{
				if(Input.GetKeyDown(KeyCode.E))
				{
					Debug.Log("Object Interacted With");
				}
			}
		}
	}
}
