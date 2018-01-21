using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (AICharacterControl))]
public class AIFollowController : MonoBehaviour {

	public float CollisionRadius;

	public AICharacterControl aiController;

	// Use this for initialization
	void Start () {
		aiController = GetComponent<AICharacterControl>();
	}
	
	// Update is called once per frame
	void Update () {
		CheckForPlayer();
	}
 
	private void CheckForPlayer()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, CollisionRadius);

		if(hitColliders.Length != 0)
		{
			foreach(Collider hit in hitColliders)
			{
				if(hit.gameObject.GetComponent<SpiritController>())
				{
					aiController.SetTarget(hit.gameObject.transform);
					return;
				}
			}

			aiController.SetTarget(null);
		}
		else
		{
			Debug.Log("No collisions!");
		}

	}
}
