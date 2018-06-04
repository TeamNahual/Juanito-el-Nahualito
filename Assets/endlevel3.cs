using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endlevel3 : MonoBehaviour {

	public AudioSource audio;
	public bool started = false;
	public bool ended = false;

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
			UIManager.instance.FadeOut();
			started = false;
			ended = false;
			StartCoroutine(StartScene());
		}
	}

	IEnumerator StartScene()
	{
		yield return new WaitForSeconds(2f);

		GameManager.instance.loadLevel("Credits");
	}
}
