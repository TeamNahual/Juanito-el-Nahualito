using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Dog_Sounds : MonoBehaviour {

	public AudioSource mySound;        //The AudioSource component
	private GameManager myManager;

	//public AudioMixerSnapshot breathingSnapshot;

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

	public int minTime;
	public int maxTime;
	private int timeSeparation;
	private int timeDiff;
	private int timer;

	private bool dogInteract = false;
	private bool dogInteractFood = false;

	// Dog Audio Clips
	public AudioClip[] dogWhimper = new AudioClip[0];

	public AudioClip[] dogBark = new AudioClip[0];

	public AudioClip[] dogBreathing = new AudioClip[0];

	public AudioClip[] dogMix = new AudioClip[0];

	// Use this for initialization
	void Start () {
		mySound = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		timer = Mathf.RoundToInt(Time.time);

		// Controls Time Randomization
		if (dogInteract == false) {
			if (timer > timeDiff) {
				timeSeparation = Mathf.RoundToInt (Random.Range (minTime, maxTime));
				timeDiff = timer + timeSeparation;
				PlaySound ();
			}
		} 
		if (dogInteractFood == true) {
			PlayBreathing ();
			//breathingSnapshot.TransitionTo (0.5f);
		} else
			return;
	}

	void OnTriggerEnter (Collider other){
		if (other.gameObject == Juanito.ins.JuanitoHuman && !Juanito.ins.hasFood){
			dogInteract = true;
			
			currentVolume = (volume + UnityEngine.Random.Range (-volumeVariance, volumeVariance));
			pitch = (1.0f + Random.Range (-pitchVariance, pitchVariance));
			mySound.pitch = pitch;

			if (dogWhimper.Length > 0 && !mySound.isPlaying) {
				mySound.PlayOneShot(dogWhimper [Random.Range (0, dogWhimper.Length)], currentVolume);
				//gameObject.GetComponent<SphereCollider>().enabled = !gameObject.GetComponent<SphereCollider> ().enabled;
			}
		}

		if (other.gameObject == Juanito.ins.JuanitoHuman && Juanito.ins.hasFood) {
			dogInteract = true;
			dogInteractFood = true;

			currentVolume = (volume + UnityEngine.Random.Range (-volumeVariance, volumeVariance));
			pitch = (1.0f + Random.Range (-pitchVariance, pitchVariance));
			mySound.pitch = pitch;

			if (dogBark.Length > 0 && !mySound.isPlaying) {
				mySound.PlayOneShot (dogBark [Random.Range (0, dogBark.Length)], currentVolume);
			}
		}
	}

	// Play Random Sounds
	void PlaySound(){
		currentVolume = (volume + UnityEngine.Random.Range (-volumeVariance, volumeVariance));
		pitch = (1.0f + Random.Range (-pitchVariance, pitchVariance));
		mySound.pitch = pitch;

		//Debug.Log("This is the dog: " + gameObject.name);
		if (dogMix.Length > 0) {
			mySound.PlayOneShot (dogMix [Random.Range (0, dogMix.Length)], currentVolume);
		}
	}
		
	void PlayBreathing(){
		currentVolume = (volume + UnityEngine.Random.Range (-volumeVariance, volumeVariance));
		pitch = (1.0f + Random.Range (-pitchVariance, pitchVariance));
		mySound.pitch = pitch;

		//Debug.Log("This is the dog: " + gameObject.name);
		if (dogBreathing.Length > 0 && !mySound.isPlaying) {
			mySound.PlayOneShot (dogBreathing [Random.Range (0, dogBreathing.Length)], currentVolume);
		}
	}
}
