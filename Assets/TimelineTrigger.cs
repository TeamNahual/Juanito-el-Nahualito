using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour {

	public PlayableDirector timeline;
	public bool playTimeline = true;
	public bool lockPlayerMovement = false;
	public GameManager myManager;

	// Use this for initialization
	void Start () {
		myManager = GameManager.instance;
		//lockPlayerMovement = myManager.isMovementLocked;

	}

	//Is called when trigger area is entered
	void OnTriggerEnter(Collider other){
		//Other is the Juanito Player Object
		if (other.gameObject == Juanito.ins.JuanitoHuman && playTimeline) {
			timeline.Play ();

			// Boolean Controls calls the Respective function on whether to lock movement
			if (lockPlayerMovement == true) {
				myManager.lockMovement ();
			}
			if (lockPlayerMovement == false) {
				myManager.unlockMovement ();
			}

			// Disables the calling the cutscene again
			playTimeline = false;
		}
	}
}
