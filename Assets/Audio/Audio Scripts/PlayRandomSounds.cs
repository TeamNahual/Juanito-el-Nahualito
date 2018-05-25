using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSounds : MonoBehaviour {

	public AudioSource mySound;        //The AudioSource component


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
	public AudioClip[] ambientSound = new AudioClip[0];

	public int minTime;
	public int maxTime;
	public int timeSeparation;
	public int timeDiff;
	public int timer;

	// Use this for initialization
	void Start () {
		mySound = gameObject.GetComponent<AudioSource>();
		//timer = (Random.Range (minTime, maxTime));
		minTime = Mathf.RoundToInt(Random.Range(1, 3));
		maxTime = Mathf.RoundToInt(Random.Range(8, 15));
	}

	// Update is called once per frame
	void Update () {
		timer = Mathf.RoundToInt(Time.time);

		// Controls Time Randomization
		if (timer > timeDiff){
			timeSeparation = Mathf.RoundToInt (Random.Range (minTime, maxTime));
			timeDiff = timer + timeSeparation;
			PlaySound ();
		}
	}


	void PlaySound(){
		currentVolume = (volume + UnityEngine.Random.Range (-volumeVariance, volumeVariance));
		pitch = (1.0f + Random.Range (-pitchVariance, pitchVariance));
		mySound.pitch = pitch;
		//Debug.Log ("Played Grass Sound");

		if (ambientSound.Length > 0) {
			mySound.PlayOneShot (ambientSound [Random.Range (0, ambientSound.Length)], currentVolume);
		} else
			Debug.LogError ("trying to play frog sound, but no frog sounds in array!");
	}
}
