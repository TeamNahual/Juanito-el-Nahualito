using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class RotationPuzzleTimeline : MonoBehaviour {

	public PlayableDirector timeline;
	public StatuePuzzleManager puzzle;

	private bool play = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(puzzle.completed && play){
			play = false;
			timeline.Play();
		}
	}
}
