using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyMesh : MonoBehaviour {

	public ButterflyPiece main;

	// Use this for initialization
	void Start () {
		main = transform.parent.gameObject.GetComponent<ButterflyPiece>();
		Physics.IgnoreCollision(Juanito.ins.JuanitoHuman.GetComponent<Collider>(), GetComponent<Collider>());
		Physics.IgnoreCollision(Juanito.ins.JuanitoSpirit.GetComponent<Collider>(), GetComponent<Collider>());

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision)
	{
		// Debug.Log(collision.gameObject.name);
		if (collision.gameObject.GetComponent<ButterflyMesh> () || 
			collision.gameObject.GetComponent<ButterflyBoundary> ()  ) 
		{
			float[] input = main.GetPlayerInput();

			if (input[0] > 0)
				main.limitRight = true;
			else if (input[0] < 0)
				main.limitLeft = true;

			if (input[1] > 0)
				main.limitForward = true;
			else if (input[1] < 0)
				main.limitBackward = true;
		}
	}

	void OnCollisionExit(Collision collision)
	{
		main.limitForward = false;
		main.limitBackward = false;
		main.limitRight = false;
		main.limitLeft = false;
	}
}
