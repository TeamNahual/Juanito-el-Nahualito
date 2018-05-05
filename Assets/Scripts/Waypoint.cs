using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	public string AnimationTag = "Default";

	// Use this for initialization
	void Start () {
		GetComponent<MeshRenderer>().enabled = false;
	}

	// Update is called once per frame
	void Update () {

	}
}
