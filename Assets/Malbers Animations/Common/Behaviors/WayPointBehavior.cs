using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MalbersAnimations;



public class WayPointBehavior : StateMachineBehaviour {
	//Parameter to set on the animator
	private string parameter = "IDInt";

	//Struct for mapping waypoint tags to IDs of animations
	[System.Serializable]
	public struct AnimationTagMap
	{
		public string tag;
		public int[] animIds;
		public AnimationTagMap(string t, int[] ids){
			this.tag = t;
			this.animIds = ids;
		}
	}
	//Array of mappings from tags to animations
	//Untagged waypoints will have random behavior
	public List<AnimationTagMap> waypointTagMap =
		new List<AnimationTagMap>(new AnimationTagMap[] {new AnimationTagMap("Default", new int[4]{1,2,3,4})});

	override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
	{
		//AnimalFollowController animalBody = animator.gameObject;
		//Default ID is 2
		int id = 2;

		//get tag of waypoint
		string tag = animator.gameObject.GetComponent<AnimalFollowController>().getWaypoint();
		// Debug.Log(tag);

		//Choose random idle id to play for the tag
		foreach(AnimationTagMap m in waypointTagMap){
			if(m.tag ==  tag){
				id = (m.animIds[Random.Range(0, m.animIds.Length)]) +  6; //must add 6 because of trasnition conditions in the animatior
				break;
			}
		}

		// Debug.Log(id);
		// Debug.Log("");

		//Make all necessary changes with the new animation ID
		animator.SetInteger(parameter, id);
		Animal animal = animator.GetComponent<Animal>();
		if (animal) animal.SetIntID(id);
	}
}
