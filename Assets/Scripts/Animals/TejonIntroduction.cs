using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;
using UnityEngine.Playables;

using MalbersAnimations.Events;
using MalbersAnimations.Utilities;
using MalbersAnimations;

public class TejonIntroduction : MonoBehaviour {

	[SerializeField]
	private Tejon tejon;
	private AnimalAIControl aiController;
	
	// [SerializeField]
	// private PlayableDirector MeetTimeline;

	[SerializeField]
	private PlayableDirector TriggerTimeline;

	void Start()
	{
		aiController = tejon.GetComponent<AnimalAIControl>();
	}

	void Update()
	{
		if(Mathf.Round ((float)TriggerTimeline.time) >= Mathf.Round ((float)TriggerTimeline.duration))
		{
			GameManager.instance.unlockMovement();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == Juanito.ins.JuanitoSpirit)
		{
			StartCoroutine(GoTo(other.GetComponent<Transform>()));
		}
	}

	IEnumerator GoTo(Transform target)
	{
		tejon.runningTask = true;
		aiController.SetTarget(target);
		
		while(aiController.agent.remainingDistance > aiController.agent.stoppingDistance)
		{
			yield return null;
		}

		tejon.runningTask = false;
		//TriggerTimeline.Stop();
		//MeetTimeline.Play();
		//GameManager.instance.lockMovement();
	}
}
