using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MalbersAnimations.Events;
using MalbersAnimations.Utilities;
using MalbersAnimations;

public class AnimalAttack : MonoBehaviour {
	Dictionary<string, int> map;
	string four = "AttackBackKick";
	string three = "AttackHornDown";
	string two  = "AttackHornUp";
	string one = "AttackRearUp";
	void Start(){
		map = new Dictionary<string, int>();
		map.Add(one, 1);
		map.Add(two, 2);
		map.Add(three, 3);
		map.Add(four, 4);
	}

	//Use this function to call an attack animation
	//Animations are {AttackBackKick, AttackHornDown, AttackHornUp, AttackRearUp},
	//	with the default being AttackRearUp
	//NOTE: Object must also have Animal script attached
	public void Attack(string index = "AttackRearUp")
	{
		gameObject.GetComponent<Animal>().SetAttack(map[index]);
	}
}
