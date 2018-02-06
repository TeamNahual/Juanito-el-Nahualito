using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterfly_piece_body : MonoBehaviour {

	public Vector3 direction;
	public string message;

	private butterfly_piece_controller controller;

	void Awake(){
		controller = GetComponentInParent <butterfly_piece_controller> ();
	}

	void OnCollisionEnter(Collision other){
		print (message);
		controller.StartMoving (direction);
	}

	void OnCollisionLeave(Collision other){
		controller.Stop ();
	}
}
