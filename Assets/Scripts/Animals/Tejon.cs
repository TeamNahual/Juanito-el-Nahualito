using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;

using MalbersAnimations.Events;
using MalbersAnimations.Utilities;
using MalbersAnimations;


public class Tejon : AnimalFollowController {

	bool runningTask = false;

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
			aiController.SetTarget(target); //follow or go to a specifc point, then set target to null when it's done following
			StartCoroutine(DigRock(target.gameObject));
		}

	}

	IEnumerator DigRock(GameObject target)
	{
		RockWallEvent controller = target.GetComponent<RockWallEvent>();

		transform.LookAt (controller.gameObject.transform);

		while(aiController.agent.remainingDistance > aiController.agent.stoppingDistance)//wait until it has reached its target
		{
			yield return null; //keeps running this in background until the above is satisfied
		}

		controller.TriggerEvent(); //triggers the event

		controller.pushing = true;

		while(!controller.disabled) //wait for the thing to finish, then finish up the task
		{
			yield return null;
		}
			
		FinishTask ();

		runningTask = false;
	}

}
