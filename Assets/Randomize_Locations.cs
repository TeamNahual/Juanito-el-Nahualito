using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomize_Locations : MonoBehaviour {

	// This Script is very heavily dependent on the PlayRandomSounds.cs script
	// and must be attatched to the same object together

	public Vector3[] sndLocation;
	private int locSize;
	private int timeSep;
	private int min;
	private int max;
	private int randomPoint;

	private int timer;

	// Use this for initialization
	void Start () {

		locSize = Mathf.RoundToInt(Random.Range(15, 20));
		sndLocation = new Vector3[locSize];

		//Specific to Act 1 in Juanito el Nahualito
		for (int i = 0; i < locSize; i++) {
			sndLocation [i].x = Random.Range(8f, 260f);
			sndLocation [i].z = Random.Range(66f, 240f);

			if (sndLocation [i].x > 8f && sndLocation [i].x < 104f) {
				sndLocation [i].y = Random.Range (128f, 158f);
			}
			if (sndLocation [i].x > 104f && sndLocation [i].x < 260f) {
				sndLocation [i].y = Random.Range (108f, 128f);
			}
			//Debug.Log ("Vector :" + sndLocation [i]);
		}	
	}
	
	// Update is called once per frame
	void Update () {
		randomPoint = Mathf.RoundToInt(Random.Range (0, sndLocation.Length));

		timer = gameObject.GetComponent<PlayRandomSounds> ().timer;
		if(timer > timeSep){
			timeSep = gameObject.GetComponent<PlayRandomSounds> ().timeDiff;
			MoveSource ();
		}
	}

	void MoveSource(){
		/*gameObject.GetComponent<Transform> ().position.x = sndLocation [randomPoint].x;
		gameObject.GetComponent<Transform> ().position.y = sndLocation [randomPoint].y;
		gameObject.GetComponent<Transform> ().position.z = sndLocation [randomPoint].z;*/

		gameObject.GetComponent<Transform> ().position = sndLocation [randomPoint];

		// Debug.Log ("Position of Object: " + gameObject.GetComponent<Transform> ().position);
		//gameObject.GetComponent<Transform>().
	}
}
