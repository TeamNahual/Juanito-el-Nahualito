using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour {

	public AnimalType ReqAnimal;

	public GameObject[] targetObjects;

	protected bool CheckPlayerDirection(GameObject player)
	{
		Vector3 fwd = player.transform.TransformDirection(Vector3.forward);
		Ray ray = new Ray(player.transform.position + Vector3.up, fwd);
		RaycastHit hit;

		Debug.DrawRay(transform.position + Vector3.up, fwd, Color.green);
		if(Physics.Raycast(ray, out hit, 1))
		{
			foreach(GameObject obj in targetObjects)
			{
				if(obj == hit.transform.gameObject)
				{
					return true;
				}
			}
		}

		return false;
	}

}
