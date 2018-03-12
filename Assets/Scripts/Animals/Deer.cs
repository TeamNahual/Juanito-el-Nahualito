using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;

using MalbersAnimations.Events;
using MalbersAnimations.Utilities;
using MalbersAnimations;

public class Deer : AnimalFollowController {

	bool runningTask = false;

	public Transform targetObject;
    

	// Use this for initialization
	void Start () {
        aiController = GetComponent<AnimalAIControl>();

        animal = AnimalType.Deer;

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
		if(target.gameObject.GetComponent<TreeEvent>())
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

		while(Vector3.Distance (transform.position, target.transform.position) > aiController.stoppingDistance + 1.5f)
		{
			yield return null;
		}

		controller.TriggerEvent();

		controller.pushing = true;

		while (controller.pushing)
		{
			yield return null;
		}

		FinishTask ();

		runningTask = false;
	}


}
