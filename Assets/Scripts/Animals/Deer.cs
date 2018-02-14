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

		animal = AnimalType.Deer;

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
		if(target.gameObject.GetComponent<StatueEvent>())
		{
			runningTask = true;
			aiController.SetTarget(target);
			StartCoroutine(RotateStatue(target.gameObject));
		}
		else if(target.gameObject.GetComponent<TreeEvent>())
		{
			runningTask = true;
			aiController.SetTarget(target);
			StartCoroutine(RotateTree(target.gameObject));
		}
		else if(target.gameObject.GetComponent<TreeRollEvent>())
		{
			runningTask = true;
			aiController.SetTarget(target);
			StartCoroutine(PushTree(target.gameObject));
		}
	}

	IEnumerator RotateStatue(GameObject target)
	{
		StatueEvent controller = target.GetComponent<StatueEvent>();

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

	IEnumerator RotateTree(GameObject target)
	{
		TreeEvent controller = target.GetComponent<TreeEvent>();

		transform.LookAt( controller.gameObject.transform);

		while(aiController.agent.remainingDistance > aiController.agent.stoppingDistance)
		{
			yield return null;
		}

		controller.TriggerEvent();

		controller.pushing = true;

		aiController.SetTarget (Juanito.ins.JuanitoSpirit.transform);

		yield return new WaitForSeconds (2);

		runningTask = false;
	}

	IEnumerator PushTree(GameObject target)
	{
		TreeRollEvent controller = target.GetComponent<TreeRollEvent>();

		transform.LookAt( controller.gameObject.transform);

		while(aiController.agent.remainingDistance > aiController.agent.stoppingDistance)
		{
			yield return null;
		}

		controller.TriggerEvent();

		controller.pushing = true;

		aiController.SetTarget (Juanito.ins.JuanitoSpirit.transform);

		yield return new WaitForSeconds (2);

		runningTask = false;
	}


}
