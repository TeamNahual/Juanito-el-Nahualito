using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class package_delivery : MonoBehaviour {

	public SpeechBubble speechBubble;

	public Transform drop_off;

	void Awake(){
		//drop_off = GetComponentInChildren <Transform> ().position;
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "package") {
			speechBubble.setDialogue (2);
			other.gameObject.GetComponent <PackageRB> ().Disable();
			other.gameObject.transform.position = drop_off.position;
			print ("delivery position is: " + drop_off);
			print ("Box position is: " + other.gameObject.transform.position);
		}
	}
}
