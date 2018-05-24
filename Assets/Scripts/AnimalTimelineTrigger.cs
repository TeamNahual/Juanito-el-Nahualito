using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

using MalbersAnimations;

public class AnimalTimelineTrigger : MonoBehaviour {

	[SerializeField] private AnimalType type;
	[SerializeField] private PlayableDirector cameraTimeline;
	[SerializeField] private PlayableDirector timeline;
	[SerializeField] private Transform start;
	[SerializeField] private Transform play;

	private float timeLineCurrent;
	private float timeLineDuration;
	
	private bool playTimeline = true;
	private bool next = false;
	private bool lockPlayerMovement = true;

	private float stopDist;

	[SerializeField] private AnimalAIControl animal;

	private Dictionary<int, System.Action> phases = new Dictionary<int, System.Action>();
	private int phase = 0;

	// Use this for initialization
	void Start () {
		timeLineDuration = Mathf.Round ((float)timeline.duration);
		Debug.Log ("Timeline Duration: " + timeLineDuration);

		phases.Add(0, GetStart);
		phases.Add(1, GetPlay);
	}

	void Update(){
		if(next)
		{
			GetPlay();
			next = false;
		}
	}

	//Is called when trigger area is entered
	void OnTriggerEnter(Collider other){
		if(playTimeline && other.gameObject == Juanito.ins.JuanitoSpirit && Juanito.ins.SpiritControl.currentFollower != null)
		{
			GetStart();
		}
	}

	private void GetStart()
	{
		cameraTimeline.Play();
		GameManager.instance.lockMovement();
		GetComponent<Collider>().enabled = false;

		Debug.Log("Start");
		animal.GetComponent<Deer>().runningTask = true;
		animal.target = start;
		animal.transform.LookAt(start);
		animal.Agent.stoppingDistance = 0.1f;
		StartCoroutine(FindStart());
	}

	IEnumerator FindStart(){

		while(animal.Agent.remainingDistance > animal.Agent.stoppingDistance)
		{
			animal.Agent.stoppingDistance = 0.1f;
			yield return null;
		}
		
		animal.GetComponent<Deer>().runningTask = false;
		next = true;
	}

	private void GetPlay()
	{
		Debug.Log("###############-- Play --###############");
		animal.GetComponent<Deer>().runningTask = true;
		animal.target = play;
		animal.Agent.stoppingDistance = 0.1f;
		StartCoroutine(FindPlay());
	}

	IEnumerator FindPlay(){
		yield return null;
		Debug.Log(animal.Agent.remainingDistance);

		while(animal.Agent.remainingDistance > animal.Agent.stoppingDistance)
		{
			animal.Agent.stoppingDistance = 4f;
			yield return null;
		}
		Debug.Log(animal.transform.position);
		Debug.Log(animal.transform.rotation.eulerAngles);
		
		timeline.Play();
		yield return new WaitForSeconds((float)timeline.duration);
		animal.GetComponent<Deer>().runningTask = false;
		GameManager.instance.unlockMovement ();
	}
}