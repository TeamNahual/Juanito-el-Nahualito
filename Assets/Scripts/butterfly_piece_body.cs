using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterfly_piece_body : MonoBehaviour {

	public string direction;

	private butterfly_piece_controller controller;

	void Awake(){
		controller = GetComponentInParent <butterfly_piece_controller> ();
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			Vector3 d;
			if (direction == "right") {
				d = transform.parent.right;
			} else if (direction == "left") {
				d = -transform.parent.right;
			} else {
				d = transform.parent.forward;
			}
			controller.StartMoving (d);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Player") {
			controller.Stop ();
		}
	}
}
