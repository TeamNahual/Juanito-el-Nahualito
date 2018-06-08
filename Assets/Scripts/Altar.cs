using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Audio;

public class Altar : MonoBehaviour {

	public bool SeashellRelic;
	public bool TorusRelic;
	public bool NecklaceRelic;

	public bool seashellHasPlayed = false;
	public bool torusHasPlayed = false;
	public bool necklaceHasPlayed = false;

	public AudioSource mySound;
	public AudioClip[] myClip = new AudioClip[0];
	public AudioMixerSnapshot[] mySnapshot = new AudioMixerSnapshot[0];

	public PlayableDirector timeline;

	private bool play = true;

	// Use this for initialization
	void Start () {
		mySound = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (play) {
			if (SeashellRelic && !seashellHasPlayed) {
				mySound.PlayOneShot (myClip [0], 1);
				mySnapshot [0].TransitionTo (1f);
				seashellHasPlayed = true;
			}

			if (TorusRelic && !torusHasPlayed) {
				mySound.PlayOneShot (myClip [0], 1);
				mySnapshot [1].TransitionTo (1f);
				torusHasPlayed = true;
			}

			if (NecklaceRelic && !necklaceHasPlayed) {
				mySound.PlayOneShot (myClip [0], 1);
				mySnapshot [2].TransitionTo (1f);
				necklaceHasPlayed = true;
			}
		}

		if(play && SeashellRelic && TorusRelic && NecklaceRelic)
		{
			play = false;
			timeline.Play();
			mySound.PlayOneShot (myClip [1], 1);
			mySnapshot [2].TransitionTo (1f);
		}
	}

	public void TriggerAltar()
	{
		
	}
}
