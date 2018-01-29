using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour {

	Renderer rend;

	bool isActive = true;
	float timeout = 3;
	float timeout_start;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!isActive)
		{
			if(Time.time - timeout_start >= timeout)
			{
				isActive = true;
				rend.enabled = true;
			}	
		}
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log(other.gameObject);

		if(other.gameObject == Juanito.ins.JuanitoHuman && isActive)
		{
			if(Juanito.ins.AddSpiritCount(1))
			{
				rend.enabled = false;
				isActive = false;
				timeout_start = Time.time;
			}
		}
	}
}
