using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TriggerNext : MonoBehaviour {

	public AudioMixerSnapshot nextSnap;
	//public AudioMixerSnapshot previous;


	void OnTriggerEnter(Collider other) {

		if(other.gameObject != Juanito.ins.JuanitoHuman)
			return;

		nextSnap.TransitionTo (.25f);
	}
}
