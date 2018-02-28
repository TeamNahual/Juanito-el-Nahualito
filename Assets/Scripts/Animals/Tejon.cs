using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;

using MalbersAnimations.Events;
using MalbersAnimations.Utilities;
using MalbersAnimations;


public class Tejon : AIFollowController {

	bool runningTask = false;

	Transform targetObject;

	// Use this for initialization
	void Start () {
        //aiController = GetComponent<AICharacterControl>();
        aiController = GetComponent<AnimalAIControl>();

        animal = AnimalType.Tejon;

		escapeLocation = new GameObject("Escape Location").transform;
		escapeLocation.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(!runningTask)
		{
			CheckForPlayer();
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

		while(controller.pushing)
		{
			yield return null;
		}
			
		aiController.SetTarget (Juanito.ins.JuanitoSpirit.transform);
	
		yield return new WaitForSeconds (2);

		runningTask = false;
	}

}
