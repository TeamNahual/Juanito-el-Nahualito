using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act_1_Change_Music : MonoBehaviour {
	public GameObject package;

	public AudioClip musicLyrics;

	void OnTriggerEnter(Collider other){
		if (other.tag == package.tag) {
			//Debug.Log ("Music Change!");
			gameObject.GetComponent<AudioSource>().clip = musicLyrics;
			gameObject.GetComponent<AudioSource> ().Play ();
			gameObject.GetComponent<SphereCollider> ().enabled = !gameObject.GetComponent<SphereCollider> ().enabled;
		}
	}
}
