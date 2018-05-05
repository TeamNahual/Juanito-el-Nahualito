using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour {

	public GameManager manager;
	Juanito juanito;
	GameObject placement;
	GameObject marker1;
	GameObject marker2;
	GameObject marker3;
	GameObject marker4;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F1)) //Load level 1
		{
			manager.loadLevel ("Act 1");
		}
		if (Input.GetKeyDown (KeyCode.F2)) //Load Cutscene between levels 1 and 2
		{
			manager.loadLevel ("Act2Cutscene");
		}
		if (Input.GetKeyDown (KeyCode.F3)) //Load Level 2
		{
			manager.loadLevel ("l2_zonetest");
		}
		if (Input.GetKeyDown (KeyCode.F4)) //Level 2 pyramid 1
		{
			manager.loadLevel ("l2_zonetest");
			this.StartCoroutine (pause("Debug Marker 1"));
		}
		if (Input.GetKeyDown (KeyCode.F5)) //Level 2 deer 2
		{
			manager.loadLevel ("l2_zonetest");
			this.StartCoroutine (pause("Debug Marker 2"));
		}
		if (Input.GetKeyDown (KeyCode.F6)) // Level 2 foxes
		{
			manager.loadLevel ("l2_zonetest");
			this.StartCoroutine (pause("Debug Marker 3"));
		}
		if (Input.GetKeyDown (KeyCode.F7)) //Level 2 pyramid 2
		{
			manager.loadLevel ("l2_zonetest");
			this.StartCoroutine (pause("Debug Marker 4"));
		}
		if (Input.GetKeyDown (KeyCode.F9)) //Load level 3
		{
			manager.loadLevel ("FlightTest");
		}
		if (Input.GetKeyDown (KeyCode.F11)) //Nudge Juanito, for when he gets stuck
		{ 
			Rigidbody rb = GameObject.Find ("ThirdPersonController").GetComponent<Rigidbody> ();
			float xForce = Random.Range (-10, 10);
			float zForce = Random.Range (-10, 10);
			rb.AddForce (xForce, 10, zForce, ForceMode.Impulse);
		}
	}

	public IEnumerator pause(string marker)
	{
		yield return null;
		juanito = GameObject.Find ("JuanitoPlayerPattern").GetComponent<Juanito> ();
		placement = juanito.JuanitoHuman;
		//Debug.Log (placement.name);
		marker1 = GameObject.Find (marker);
		//Debug.Log (marker1.name);
		placement.transform.position = marker1.transform.position;
	}
}
