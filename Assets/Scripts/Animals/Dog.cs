using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;

using MalbersAnimations.Events;
using MalbersAnimations.Utilities;
using MalbersAnimations;


public class Dog : AnimalFollowController
{

    bool runningTask = false;

    Transform targetObject;

    // Use this for initialization
    void Start()
    {
        //aiController = GetComponent<AICharacterControl>();
        aiController = GetComponent<AnimalAIControl>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!runningTask)
        {
            UpdateAnimal();
        }
    }
    protected void UpdateAnimal()
    {
        if (!traveling) //if deer is standing still at waypoint, checks for player
            CheckForPlayer();
        else
        {
            if (currentWaypoint != null) //if traveling and does have a way point
            {
                //check distance between animal and waypoint
                if (Vector3.Distance(transform.position, currentWaypoint.transform.position) < aiController.stoppingDistance + 1.5f)
                {
                    //says animal arrived at waypoint
                    //Debug.Log ("Reached " + currentWaypoint.name);
                    traveling = false;
                    currentWaypoint = null;
                }
            }
        }
        //if the dog is following a target
        if (following)
        {
            if (aiController.target.name == "food")//if the target is food eventually eat the food
            {
                Debug.Log("Dog is going to food");
            }
            else if (!Juanito.ins.hasFood)//if the target is Juanito and he no longer has food, stop following him.
            {
                currentWaypoint = WayPoints[wayPointCount];
                aiController.SetTarget(currentWaypoint.transform);
                wayPointCount = (wayPointCount + 1 > WayPoints.Length - 1 ? 0 : wayPointCount + 1);
                traveling = true;
                following = false;
            }
        }
    }
    protected void CheckForPlayer()
    {
        //sphere around, with collision radius mess around to see good radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, CollisionRadius);

        if (hitColliders.Length != 0)
        {
            foreach (Collider hit in hitColliders)
            {
                if (hit.gameObject == Juanito.ins.JuanitoHuman)
                {
                    if (!Juanito.ins.hasFood)//if Juanito doesn't have food leave him
                    {
                        currentWaypoint = WayPoints[wayPointCount];
                        aiController.SetTarget(currentWaypoint.transform);
                        wayPointCount = (wayPointCount + 1 > WayPoints.Length - 1 ? 0 : wayPointCount + 1);
                        traveling = true;
                        return;
                    }
                    else// if Juanito has food follow him
                    {
                        aiController.SetTarget(hit.gameObject.transform);
                        Juanito.ins.SpiritControl.currentFollower = transform.gameObject;
                        following = true;
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
    public void RunTask(Transform target)
    {
        if (target.gameObject.GetComponent<PullyEvent>())
        {
            runningTask = true;
            aiController.SetTarget(target); //follow or go to a specifc point, then set target to null when it's done following
            StartCoroutine(MoveTo(target.gameObject));
        }

    }

    IEnumerator MoveTo(GameObject target)
    {
        PullyEvent controller = target.GetComponent<PullyEvent>();

        transform.LookAt(controller.gameObject.transform);

        while (aiController.agent.remainingDistance > aiController.agent.stoppingDistance)//wait until it has reached its target
        {
            yield return null; //keeps running this in background until the above is satisfied
        }

        controller.TriggerEvent(); //triggers the event

        controller.moving = true;

        while (!controller.disabled) //wait for the thing to finish, then finish up the task
        {
            yield return null;
        }

        FinishTask();

        runningTask = false;
    }


}