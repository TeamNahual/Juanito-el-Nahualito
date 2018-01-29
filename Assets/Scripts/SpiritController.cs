using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		InteractWithObject();
	}

	void InteractWithObject()
	{
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Ray ray = new Ray(fwd,transform.position);
		RaycastHit hit;

		Debug.DrawRay(transform.position, fwd, Color.green);
		if(Physics.Raycast(ray, out hit, 1))
		{
			EventObject obj = hit.transform.gameObject.GetComponent<EventObject>();

			if(obj)
			{
				if(Input.GetKeyDown(KeyCode.E))
				{
					Debug.Log("Object Interacted With");
					Vector3 dir = (2 * transform.position - hit.point);

					obj.transform.position = Vector3.Lerp(obj.transform.position, dir, 1);
				}
			}
		}
	}
}
