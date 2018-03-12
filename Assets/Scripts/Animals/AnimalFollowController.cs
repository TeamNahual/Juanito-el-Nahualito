using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

using MalbersAnimations.Events;
using MalbersAnimations.Utilities;
using MalbersAnimations;

public class AnimalFollowController : MonoBehaviour {

	public AnimalType animal;
	public float CollisionRadius;
	public GameObject[] WayPoints;

	//[HideInInspector]
	public AnimalAIControl aiController;

	protected bool following;
	public bool traveling;
	protected int wayPointCount = 0;
	protected GameObject currentWaypoint;

	// Use this for initialization
	void Start () {
		aiController = GetComponent<AnimalAIControl>();
	}

	// Update is called once per frame
	void Update () {
		UpdateAnimal ();
	}

	protected void UpdateAnimal()
	{
		if (!traveling)
			CheckForPlayer ();
		else
		{
			if (currentWaypoint != null)
			{
				if (Vector3.Distance (transform.position, currentWaypoint.transform.position) < aiController.stoppingDistance + 1.5f)
				{
					//Debug.Log ("Reached " + currentWaypoint.name);
					traveling = false;
					currentWaypoint = null;
				}
			}
		}

		if (following && !Juanito.ins.SpiritState)
		{
			currentWaypoint = WayPoints [wayPointCount];
			aiController.SetTarget (currentWaypoint.transform);
			wayPointCount = (wayPointCount + 1 > WayPoints.Length - 1? 0 : wayPointCount + 1);
			traveling = true;
			following = false;
		}
	}

	protected void FinishTask()
	{
		if (Juanito.ins.SpiritState)
		{
			aiController.SetTarget (Juanito.ins.JuanitoSpirit.transform);
		} else
		{
			currentWaypoint = WayPoints [wayPointCount];
			aiController.SetTarget (currentWaypoint.transform);
			wayPointCount = (wayPointCount + 1 > WayPoints.Length - 1? 0 : wayPointCount + 1);
			traveling = true;
		}
	}

	protected void CheckForPlayer()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, CollisionRadius);

		if (hitColliders.Length != 0)
		{
			foreach (Collider hit in hitColliders)
			{
				if (!Juanito.ins.SpiritState)
				{
					// Flee to Waypoint if Juanito is Human
					if (hit.gameObject == Juanito.ins.JuanitoHuman)
					{
						// aiController.Agent.enabled = true;
						// aiController.isMoving = false;
						currentWaypoint = WayPoints [wayPointCount];
						aiController.SetTarget (currentWaypoint.transform);
						wayPointCount = (wayPointCount + 1 > WayPoints.Length - 1? 0 : wayPointCount + 1);
						traveling = true;
						return;
					}

				} else
				{
					// Follow Juanito if Juanito is Spirit
					if (hit.gameObject == Juanito.ins.JuanitoSpirit)
					{
						// aiController.Agent.enabled = true;
						// aiController.isMoving = false;
						aiController.SetTarget (hit.gameObject.transform);
						following = true;
						Juanito.ins.SpiritControl.currentFollower = transform.gameObject;
						return;
					}
				}
			}
		}
		else
		{
			Debug.Log("No collisions!");
		}

		//following = false;
		//aiController.SetTarget(null);

	} 

}