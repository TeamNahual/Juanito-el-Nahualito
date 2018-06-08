using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TriggerNext : MonoBehaviour {

	public AudioMixerSnapshot nextSnap;
	//public AudioMixerSnapshot previous;
	public float transitionTime = 1f;


	void OnTriggerEnter(Collider other) {

		if(other.gameObject != Juanito.ins.JuanitoHuman)
			return;

		nextSnap.TransitionTo (transitionTime);

		gameObject.GetComponent<BoxCollider> ().enabled = !gameObject.GetComponent<BoxCollider> ().enabled;
	}
}
