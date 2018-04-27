using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MalbersAnimations.Events;
using MalbersAnimations.Utilities;
using MalbersAnimations;

public class AnimalAttack : MonoBehaviour {
	//Use this function to call an attack animation
	//Animations are 1-4, with the default being 1
	//NOTE: Object must also have Animal script attached
	void Attack(int index = 1)
	{
		Debug.Log("attack " + index);
		gameObject.GetComponent<Animal>().SetAttack(index);
	}
}
