using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterfly_piece_tip : MonoBehaviour {

	private butterfly_piece_controller cont;

	void Awake(){
		cont = GetComponent <butterfly_piece_controller> ();
	}
}
