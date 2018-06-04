using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class Act3TimelineTrigger : MonoBehaviour {

	public PlayableDirector timeline;
	public bool playTimeline = true;

    public CinemachineVirtualCamera oldCamera;


	// Use this for initialization
	void Start () {
		
	}

	void Update(){
		
	}

	//Is called when trigger area is entered
	void OnTriggerEnter(Collider other){
        timeline.Play();

        oldCamera.Priority = 0;
        // Disables the calling the cutscene again
        playTimeline = false;
    }

	
}
	
