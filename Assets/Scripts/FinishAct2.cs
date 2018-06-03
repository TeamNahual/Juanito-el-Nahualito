using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FinishAct2 : MonoBehaviour {

	public PlayableDirector timeline;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Mathf.Round ((float)timeline.time) >= Mathf.Round ((float)timeline.duration))
		{
			GameManager.instance.loadLevel("Act3Cutscene");
		}
	}
}
