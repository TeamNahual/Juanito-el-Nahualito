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
	public AudioClip[] frog = new AudioClip[0];

	public float minTime;
	public float maxTime;
	public int timeSeparation;
	public float timer;

	// Use this for initialization
	void Start () {
		mySound = gameObject.GetComponent<AudioSource>();
		timer = (Random.Range (minTime, maxTime));
	}

	// Update is called once per frame
	void Update () {
		if (timer >= maxTime) {
			timer = (Random.Range (minTime, maxTime));
		}
		timer += Time.deltaTime;
		if (Mathf.RoundToInt(timer) % Mathf.RoundToInt(Random.Range(6, 10)) == 0){
			if (mySound.isPlaying == false){
				PlayFrogs ();
			}
		}
	}


	void PlayFrogs(){
		currentVolume = (volume + UnityEngine.Random.Range (-volumeVariance, volumeVariance));
		pitch = (1.0f + Random.Range (-pitchVariance, pitchVariance));
		mySound.pitch = pitch;
		//Debug.Log ("Played Grass Sound");

		if (frog.Length > 0) {
				mySound.PlayOneShot (frog [Random.Range (0, frog.Length)], currentVolume);
		} else
			Debug.LogError ("trying to play frog sound, but no frog sounds in array!");
	}

	void FrogCroak(){
		timeSeparation = Mathf.RoundToInt(Random.Range (minTime, maxTime));
		Invoke ("PlayFrogs", timeSeparation);
	}
}
