using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

	void Start()
	{

	}

	void OnCollisionStay(Collision other)
	{
		if(other.collider.tag == "Player")
		{
			other.gameObject.transform.parent = transform.parent;
		}
	}

	void OnCollisionExit(Collision other)
	{
		if(other.collider.tag == "Player")
		{
			other.gameObject.transform.parent = null;
		}
	}
}
