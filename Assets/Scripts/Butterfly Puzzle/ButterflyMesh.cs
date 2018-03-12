using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyMesh : MonoBehaviour {

	public ButterflyPiece main;

	// Use this for initialization
	void Start () {
		main = transform.parent.gameObject.GetComponent<ButterflyPiece>();
		Physics.IgnoreCollision(Juanito.ins.JuanitoHuman.GetComponent<Collider>(), GetComponent<Collider>());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<ButterflyMesh> ()) 
		{
			float vInput = main.GetPlayerInput () [1];

			if (vInput > 0)
				main.limitForward = true;
			else if (vInput < 0)
				main.limitBackward = true;
		}
	}

	void OnCollisionExit(Collision collision)
	{
		main.limitForward = false;
		main.limitBackward = false;
	}
}
