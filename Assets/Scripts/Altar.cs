using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Altar : MonoBehaviour {

	public bool SeashellRelic;
	public bool TorusRelic;
	public bool NecklaceRelic;

	public PlayableDirector timeline;

	private bool play = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(play && SeashellRelic && TorusRelic && NecklaceRelic)
		{
			play = false;
			timeline.Play();
		}
	}

	public void TriggerAltar()
	{
		
	}
}
