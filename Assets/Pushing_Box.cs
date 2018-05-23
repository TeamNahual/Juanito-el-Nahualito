using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushing_Box : MonoBehaviour {

	private AudioSource pushingBox;

	// Use this for initialization
	void Start () {
		pushingBox = gameObject.GetComponent<AudioSource> ();
		pushingBox.Play ();	
		pushingBox.Pause ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Juanito.ins.isPushing == true) {
			pushingBox.UnPause ();
		}
		if (Juanito.ins.isPushing == false) {
			pushingBox.Pause ();
		}
	}
}
