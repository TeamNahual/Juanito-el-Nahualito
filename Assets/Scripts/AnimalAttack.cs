using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MalbersAnimations.Events;
using MalbersAnimations.Utilities;
using MalbersAnimations;

public class AnimalAttack : MonoBehaviour {

	//Use this function to call an attack animation
	//Animations are {AttackBackKick, AttackHornDown, AttackHornUp, AttackRearUp},
	//	with the default being AttackRearUp
	//NOTE: Object must also have Animal script attached
	public void Attack(string attack)
	{

		Debug.Log(attack);
		if(attack == "AttackRearUp" || attack == "AttackBiteL"){
			Debug.Log("wow");
			gameObject.GetComponent<Animal>().SetAttack(1);
		}
		else if(attack == "AttackHornUp" || attack == "AttackBiteR"){
			gameObject.GetComponent<Animal>().SetAttack(2);
		}
		else if(attack == "AttackHornDown" || attack == "AttackPaws"){
			gameObject.GetComponent<Animal>().SetAttack(3);
		}
		else if(attack == "AttackBackKick" || attack == "AttackJump"){
			gameObject.GetComponent<Animal>().SetAttack(4);
		}
	}
}
