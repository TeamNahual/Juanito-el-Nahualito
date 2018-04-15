using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulley_switch_manager : MonoBehaviour {

	private pulley_manager manager;
	private bool overlap = false;

	void Awake(){
		manager = GetComponentInParent <pulley_manager> ();
	}
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject == Juanito.ins.JuanitoHuman)
			overlap = true;
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject == Juanito.ins.JuanitoHuman)
			overlap = false;
	}

	void Update(){
		if (overlap && Input.GetKeyDown (KeyCode.E)) {
			manager.MovePlatforms ();
		}
	}
}
