using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

using MalbersAnimations;

public class AnimalEventTrigger : MonoBehaviour {

	//Timeline associated with Event
	public PlayableDirector timeline;
	//Lock movement during cutscene
	public bool lockMovement = false;
	public float stopDist;

	private GameManager myManager;
	private bool activated = false;
	private float TimelineTime = 0;
	private float TimelineLength;
	private GameObject animal;


	void Start()
	{
		myManager = GameManager.instance;

		if(lockMovement) TimelineLength = Mathf.Round((float)timeline.duration); 
	}

	void Update()
	{
		if(lockMovement)
		{
			TimelineTime = Mathf.Round((float)timeline.time);

			if(TimelineTime == TimelineLength)
			{
				myManager.unlockMovement();
				lockMovement = false;
				animal.GetComponent<Deer>().runningTask = false;
				animal.GetComponent<Tejon>().runningTask = false;
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		animal = Juanito.ins.SpiritControl.currentFollower;

		if(other.gameObject == Juanito.ins.JuanitoSpirit && !activated)
		{
			if(animal.GetComponent<Deer>())
			{
				if(lockMovement) myManager.lockMovement();
				animal.GetComponent<Deer>().runningTask = true;
				StartCoroutine(DeerEvent(animal));
				activated = true;
			}
			else if(animal.GetComponent<Tejon>())
			{
				if(lockMovement) myManager.lockMovement();
				animal.GetComponent<Tejon>().runningTask = true;
				StartCoroutine(TejonEvent(animal));
				activated = true;
			}
		}
	}

	IEnumerator DeerEvent(GameObject deer)
	{
		AnimalAIControl ai = deer.GetComponent<AnimalAIControl>();
		
		ai.SetTarget(transform);
		deer.transform.LookAt(transform);
		
		float dist = ai.agent.stoppingDistance;
		ai.agent.stoppingDistance = stopDist;

		while(ai.agent.remainingDistance > ai.agent.stoppingDistance)
		{
			Debug.Log(ai.agent.remainingDistance + ", " + ai.agent.stoppingDistance);
			yield return null;
		}

		timeline.Play();
		
		ai.agent.stoppingDistance = dist;
	}

	IEnumerator TejonEvent(GameObject tejon)
	{
		AnimalAIControl ai = tejon.GetComponent<AnimalAIControl>();

		ai.SetTarget(transform);
		tejon.transform.LookAt(transform);
		
		float dist = ai.agent.stoppingDistance;
		ai.agent.stoppingDistance = stopDist;

		while(ai.agent.remainingDistance > ai.agent.stoppingDistance)
		{
			Debug.Log(ai.agent.remainingDistance + ", " + ai.agent.stoppingDistance);
			yield return null;
		}

		timeline.Play();
		
		ai.agent.stoppingDistance = dist;
	}
}