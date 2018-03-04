using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

using MalbersAnimations.Events;
using MalbersAnimations.Utilities;
using MalbersAnimations;

//[RequireComponent(typeof (AICharacterControl))]
public class AIFollowController : MonoBehaviour {

	public AnimalType animal;
	public float CollisionRadius;

	[HideInInspector]
	//public AICharacterControl aiController;
    public AnimalAIControl aiController;

    protected Transform escapeLocation;
	protected bool following;
	// Use this for initialization
	void Start () {
        //aiController = GetComponent<AICharacterControl>();
        aiController = GetComponent<AnimalAIControl>();
        escapeLocation = new GameObject("Escape Location").transform;
		escapeLocation.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () {
		CheckForPlayer();
	}
 
	protected void CheckForPlayer()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, CollisionRadius);

		if(hitColliders.Length != 0)
		{
			foreach(Collider hit in hitColliders)
			{
				if(hit.gameObject.GetComponent<SpiritController>())
				{
					aiController.SetTarget(hit.gameObject.transform);
					following = true;
					Juanito.ins.SpiritControl.currentFollower = transform.gameObject;
					return;
				}
				else if(hit.gameObject.GetComponent<ThirdPersonUserControl>())
				{
					FleePlayer(hit.gameObject.transform);
					return;
				}
			}
			// aiController.SetTarget(null);
		}
		else
		{
			Debug.Log("No collisions!");
		}

		following = false;
		aiController.SetTarget(null);

	}

	protected void FleePlayer(Transform fleeTarget)
	{
		Vector3 directionToPlayer = transform.position - fleeTarget.position;
		Vector3 checkPos = transform.position + directionToPlayer * 2;

		NavMeshHit navHitForward, navHitLeft, navHitRight, navHitToward;

		if(NavMesh.SamplePosition(checkPos, out navHitForward, 3.0f, NavMesh.AllAreas))
		{
			//
		}
		
		if(NavMesh.SamplePosition(FleeLeft(checkPos), out navHitLeft, 3.0f, NavMesh.AllAreas))
		{
			//
		}
		
		if(NavMesh.SamplePosition(FleeRight(checkPos), out navHitRight, 3.0f, NavMesh.AllAreas))
		{
			// 
		}

		if(NavMesh.SamplePosition(FleeToward(checkPos), out navHitToward, 3.0f, NavMesh.AllAreas))
		{
			// 
		}

		float scoreForward = Vector3.Distance(navHitForward.position, transform.position);
		float scoreLeft = Vector3.Distance(navHitLeft.position, transform.position);
		float scoreRight = Vector3.Distance(navHitRight.position, transform.position);
		float scoreToward = Vector3.Distance(navHitToward.position, transform.position);


		scoreForward = (scoreForward == Mathf.Infinity ? 0 : scoreForward);
		scoreLeft = (scoreLeft == Mathf.Infinity ? 0 : scoreLeft);
		scoreRight = (scoreRight == Mathf.Infinity ? 0 : scoreRight);
		scoreToward = (scoreToward == Mathf.Infinity ? 0 : scoreToward);

		float maxScore = Mathf.Max(new float[] {scoreForward, scoreLeft, scoreRight, scoreToward});
		
		if(maxScore != 0)
		{
			if(maxScore == scoreForward && navHitForward.hit)
			{
				escapeLocation.position = navHitForward.position;
			}
			else if(maxScore == scoreLeft && navHitLeft.hit)
			{
				escapeLocation.position = navHitLeft.position;
			}
			else if(maxScore == scoreRight && navHitRight.hit)
			{
				escapeLocation.position = navHitRight.position;
			}
			else if(maxScore == scoreToward && navHitToward.hit)
			{
				escapeLocation.position = navHitToward.position;
			}
		}

		if(escapeLocation)
			aiController.SetTarget(escapeLocation);
	}

	protected Vector3 FleeToward(Vector3 targetPos)
	{	
		Vector3 dir =  targetPos - transform.position;
		dir = Quaternion.Euler(0, 50, 0) * dir;

		return dir + transform.position;
	}

	protected Vector3 FleeRight(Vector3 targetPos)
	{
		Vector3 dir =  targetPos - transform.position;
		dir = Quaternion.Euler(0, 44, 0) * dir;
		return dir + transform.position;
	}

	protected Vector3 FleeLeft(Vector3 targetPos)
	{
		Vector3 dir =  targetPos - transform.position;
		dir = Quaternion.Euler(0, -46, 0) * dir;
		return dir + transform.position;
	}
}