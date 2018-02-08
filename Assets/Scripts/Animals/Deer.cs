using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;

public class Deer : AIFollowController {

	bool runningTask = false;

	Transform targetObject;

	// Use this for initialization
	void Start () {
		aiController = GetComponent<AICharacterControl>();

		escapeLocation = new GameObject().transform;
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
		if(target.gameObject.GetComponent<StatueEvent>())
		{
			statue = target.gameObject.GetComponent<StatueEvent>();
			runningTask = true;
			aiController.SetTarget(target);
			StartCoroutine(RotateStatue(transform.gameObject));
		}
	}

	IEnumerator RotateStatue(GameObject target)
	{
		StatueEvent controller = target.GetComponent<StatueEvent>();

		while(aiController.agent.remainingDistance > aiController.agent.stoppingDistance)
		{
			yield return null;
		}

		controller.TriggerEvent();

		while(controller.pushing)
			yield return null;

		runningTask = false;

	}
}
