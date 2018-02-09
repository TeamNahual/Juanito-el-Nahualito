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
			runningTask = true;
			aiController.SetTarget(target);
			StartCoroutine(RotateStatue(target.gameObject));
		}
	}

	IEnumerator RotateStatue(GameObject target)
	{
		StatueEvent controller = target.GetComponent<StatueEvent>();

		while(aiController.agent.remainingDistance > aiController.agent.stoppingDistance)
		{
			Debug.Log("walking to statue");
			yield return null;
		}

		controller.TriggerEvent();

		controller.pushing = true;

		while(controller.pushing)
		{
			Debug.Log("pushing");
			yield return null;
		}

		runningTask = false;

	}
}
