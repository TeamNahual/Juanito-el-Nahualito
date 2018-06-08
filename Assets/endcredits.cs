using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endcredits : MonoBehaviour {

	public AudioSource audio;
	public bool started = false;
	public bool ended = false;

	// public GameObject KeyText;
	private bool state = false;
	private bool interactable = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(!started)
		{
			if(audio.isPlaying)
				started = true;
		}

		if(started && !ended)
		{
			if(!audio.isPlaying)
				ended = true;
		}

		if(started && ended)
		{
			started = false;
			ended = false;
			GameManager.instance.loadLevel("Menu");
		}


		if (Input.anyKeyDown && interactable)
		{
			if (!state)
			{
				state = true;
				// KeyText.SetActive (true);
			}
			else
			{
				GameManager.instance.loadLevel("Menu");
			}
		}
	}

}
