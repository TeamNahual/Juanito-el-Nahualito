using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterfly_piece_tip : MonoBehaviour {

	//-1 to rotate to the left and 1 to rotate to the right
	public int rotateFlag = 1;

	private butterfly_piece_controller cont;
	// private Transform focusPoint;

	void Awake(){
		cont = GetComponentInParent <butterfly_piece_controller> ();
		// focusPoint = GetComponentInChildren <Transform> ();
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			cont.StartRotating (rotateFlag/*, focusPoint*/);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Player") {
			cont.Stop ();
		}
	}
}
