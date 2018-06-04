using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Manage_Default_Snapshot : MonoBehaviour {
	
	public AudioMixerSnapshot[] Snapshots;
	// Use this for initialization
	void Start () {
		if (gameObject.tag == "Act 1") {
			if (Snapshots [0].name.Contains ("Act 1")) {
				Snapshots [0].TransitionTo (0.5f);
			}
		}
		if (gameObject.tag == "Act 2") {
			if (Snapshots [0].name.Contains ("Act 2")) {
				Snapshots [0].TransitionTo (0.5f);
			}
		}
		if (gameObject.tag == "Act 3") {
			if (Snapshots [0].name.Contains ("Act 3")) {
				Snapshots [0].TransitionTo (0.5f);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
