using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		if (gameObject.tag == "Pidgeon") {
			locSize = Mathf.RoundToInt (Random.Range (15, 20));
		}
		if (gameObject.tag == "Bird") {
			locSize = Mathf.RoundToInt (Random.Range (30, 50));
		}

		sndLocation = new Vector3[locSize];

		//Specific to Act 1 in Juanito el Nahualito
		if(gameObject.tag == "Pidgeon"){
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

		//Specific to Act 2 in Juanito el Nahualito
		if (gameObject.tag == "Bird") {
			for (int i = 0; i < locSize; i++) {
				sndLocation [i].x = Random.Range(-172f, 213f);
				sndLocation [i].z = Random.Range(-134f, 230f);
				sndLocation [i].y = Random.Range (18f, 34f); 

				if (sndLocation [i].x < 114f && sndLocation [i].x > -260f &&
					sndLocation[i].z < 230 && sndLocation[i].z > 40) {
					sndLocation [i].x = Random.Range(-125f, 148f);
					sndLocation [i].z = Random.Range(-95f, 15f);
				}
				//Debug.Log ("Vector :" + sndLocation [i]);
			}	
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
