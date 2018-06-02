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
	[SerializeField] private float Distance = 3.5f;

	
	private bool update = false;
	private int state = 0;

	private AnimalAIControl animal;

	void Start () {}

	void Update(){
		if(update)
		{
			if(state == 0)
				StartCoroutine(GoTo(start, 0.2f));
			else if(state == 1)
				StartCoroutine(GoTo(play, Distance));
			else if(state == 2)
				StartCoroutine(PlayTimeline());

			state++;
			update = false;
		}
	}

	//Is called when trigger area is entered
	void OnTriggerEnter(Collider other){
		if(other.gameObject == Juanito.ins.JuanitoSpirit && Juanito.ins.SpiritControl.currentFollower != null)
		{
			StartCoroutine(PlayCamera());
			animal = Juanito.ins.SpiritControl.currentFollower.GetComponent<AnimalAIControl>();
			update = true;
		}
	}

	IEnumerator GoTo(Transform target, float dist)
	{
		SetTask(true);
		animal.target = target;
		animal.Agent.stoppingDistance = dist;
		yield return null;

		while(animal.Agent.remainingDistance > animal.Agent.stoppingDistance)
		{
			animal.agent.stoppingDistance = dist;
			yield return null;
		}

		SetTask(false);
		update = true;
	}

	IEnumerator PlayTimeline()
	{
		SetTask(true);
		animal.target = play;
		timeline.Play();

		yield return new WaitForSeconds((float)timeline.duration);
		SetTask(false);
	}

	IEnumerator PlayCamera()
	{
		GetComponent<Collider>().enabled = false;
		GameManager.instance.lockMovement();
		cameraTimeline.Play();

		yield return new WaitForSeconds((float)cameraTimeline.duration);
		GameManager.instance.unlockMovement();
		enabled = false;
	}

	private void SetTask(bool b)
	{
		if(type == AnimalType.Deer)
			animal.GetComponent<Deer>().runningTask = b;
		else if(type == AnimalType.Tejon)
			animal.GetComponent<Tejon>().runningTask = b;
	}
}