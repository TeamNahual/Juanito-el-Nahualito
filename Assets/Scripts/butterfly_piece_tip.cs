using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterfly_piece_tip : MonoBehaviour {

	//-1 to rotate to the left and 1 to rotate to the right
	public int rotateFlag = 1;

	private butterfly_piece_controller cont;

	void Awake(){
		cont = GetComponentInParent <butterfly_piece_controller> ();
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Juanito") {
			cont.StartRotating (rotateFlag);
		}
	}

	void OnCollisionExit(Collision other){
		if (other.gameObject.tag == "Juanito") {
			cont.Stop ();
		}
	}
}
