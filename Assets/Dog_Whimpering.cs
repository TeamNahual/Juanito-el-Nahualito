using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog_Whimpering : MonoBehaviour {

	public AudioSource mySound;        //The AudioSource component
	private GameManager myManager;


	[Space(5.0f)]
	private float currentVolume;
	[Range(0.0f,1.0f)]
	public float volume = 1.0f;             //Volume slider bar; set this between 0 and 1 in the Inspector.
	[Range(0.0f,0.2f)]
	public float volumeVariance = 0.04f;    //Variance in volume levels per footstep; set this between 0.0 and 0.2 in the inspector. Default is 0.04f.
	private float pitch;
	[Range(0.0f,0.2f)]
	public float pitchVariance = 0.08f;     //Variance in pitch levels per footstep; set this between 0.0 and 0.2 in the inspector. Default is 0.08f.
	[Space(5.0f)]
	public AudioClip[] dogWhimper = new AudioClip[0];

	public AudioClip[] dogBark = new AudioClip[0];

	// Use this for initialization
	void Start () {
		mySound = gameObject.GetComponent<AudioSource>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider other){
		if (other.gameObject == Juanito.ins.JuanitoHuman && !Juanito.ins.hasFood){
			
			currentVolume = (volume + UnityEngine.Random.Range (-volumeVariance, volumeVariance));
			pitch = (1.0f + Random.Range (-pitchVariance, pitchVariance));
			mySound.pitch = pitch;

			if (dogWhimper.Length > 0) {
				mySound.PlayOneShot(dogWhimper [Random.Range (0, dogWhimper.Length)], currentVolume);
				//gameObject.GetComponent<SphereCollider>().enabled = !gameObject.GetComponent<SphereCollider> ().enabled;
			} else
				Debug.LogError ("trying to play dog sound, but no dog sounds in array!");
		}

		if (other.gameObject == Juanito.ins.JuanitoHuman && Juanito.ins.hasFood) {

			currentVolume = (volume + UnityEngine.Random.Range (-volumeVariance, volumeVariance));
			pitch = (1.0f + Random.Range (-pitchVariance, pitchVariance));
			mySound.pitch = pitch;

			if (dogBark.Length > 0) {
				mySound.PlayOneShot (dogBark [Random.Range (0, dogBark.Length)], currentVolume);
			}
		}
	}
}
