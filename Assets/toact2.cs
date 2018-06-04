using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class toact2 : MonoBehaviour {

	public PlayableDirector timeline;
	float timeLineDuration;
	private float timeLineCurrent;


	// Use this for initialization
	void Start () {
		timeLineDuration = (float) timeline.duration;
	}
	
	// Update is called once per frame
	void Update () {
		timeLineCurrent = (float) timeline.time;

		if (timeLineCurrent >= timeLineDuration - 0.5f) {
			GameManager.instance.loadLevel("Act2Cutscene");
		}
	}
}
