using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour {

	public PlayableDirector timeline;
	public bool playTimeline = true;
	public bool lockPlayerMovement = false;
	public PlayerType playerType;
	private GameManager myManager;
	private float timeLineCurrent;
	private float timeLineDuration;

	// Use this for initialization
	void Start () {
		myManager = GameManager.instance;
		//lockPlayerMovement = myManager.isMovementLocked;
		//timeLineCurrent = Mathf.Round ((float)timeline.time);
		if (lockPlayerMovement == true) {
			timeLineDuration = (float) timeline.duration;
			Debug.Log ("Timeline Duration: " + timeLineDuration);
		}
	}

	void Update(){
		if(lockPlayerMovement == true){
			timeLineCurrent = (float) timeline.time;
			// Debug.Log ("Timeline Time: " + timeLineCurrent + " Timeline Duration: " + timeLineDuration);

			if (timeLineCurrent >= timeLineDuration - 0.5f) {
				myManager.unlockMovement ();
				lockPlayerMovement = false;
				//Debug.Log ("Called Unlocking");
			}
		}
	}

	//Is called when trigger area is entered
	void OnTriggerEnter(Collider other){
		if (playerType == PlayerType.Human)
			PlayHumanTimeline (other);
		else if (playerType == PlayerType.Spirit)
			PlaySpiritTimeline(other);
	}

	void PlayHumanTimeline(Collider other)
	{
		if (other.gameObject == Juanito.ins.JuanitoHuman && playTimeline) {
			timeline.Play ();

			// Boolean Controls calls the Respective function on whether to lock movement
			if (lockPlayerMovement == true) {
				myManager.lockMovement ();
			}


			// Disables the calling the cutscene again
			playTimeline = false;
		}
	}

	void PlaySpiritTimeline(Collider other)
	{
		if (other.gameObject == Juanito.ins.JuanitoSpirit && playTimeline) {
			timeline.Play ();

			// Boolean Controls calls the Respective function on whether to lock movement
			if (lockPlayerMovement == true) {
				myManager.lockMovement ();
			}

			// Disables the calling the cutscene again
			playTimeline = false;
		}
	}
}
	
public enum PlayerType
{
	Human,
	Spirit
}