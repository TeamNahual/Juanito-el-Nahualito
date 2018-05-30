using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;

using MalbersAnimations.Events;
using MalbersAnimations.Utilities;
using MalbersAnimations;


public class Tejon : AnimalFollowController {

	public bool runningTask = false;

	Transform targetObject;

	// Use this for initialization
	void Start () {
        //aiController = GetComponent<AICharacterControl>();
        aiController = GetComponent<AnimalAIControl>();

	}
	
	// Update is called once per frame
	void Update () {
		if(!runningTask)
		{
			UpdateAnimal ();
		}
	}

	public void RunTask(Transform target)
	{
		if(target.gameObject.GetComponent<RockWallEvent>())
		{
			runningTask = true;
			aiController.SetTarget(target);
			StartCoroutine(DigRock(target.gameObject));
		}

	}

	IEnumerator DigRock(GameObject target)
	{
		RockWallEvent controller = target.GetComponent<RockWallEvent>();

		transform.LookAt (controller.gameObject.transform);

		while(aiController.agent.remainingDistance > aiController.agent.stoppingDistance)
		{
			yield return null;
		}

		controller.TriggerEvent();

		controller.pushing = true;

		while(!controller.disabled)
		{
			yield return null;
		}
			
		FinishTask ();

		runningTask = false;
	}

}
