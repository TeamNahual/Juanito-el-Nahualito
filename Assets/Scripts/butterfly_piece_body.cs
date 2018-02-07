using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterfly_piece_body : MonoBehaviour {

	public Vector3 direction;

	private butterfly_piece_controller controller;

	void Awake(){
		controller = GetComponentInParent <butterfly_piece_controller> ();
	}

	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag == "Juanito"){
			controller.StartMoving (direction);
		}
	}

	void OnCollisionExit(Collision other){
		if (other.gameObject.tag == "Juanito") {
			controller.Stop ();
		}
	}
}
