using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterfly_puzzle_manager : MonoBehaviour {

	private int count = 0;
	private int activePoints = 0;
	private bool allActive = false;

	public void IncreaseCount(){
		++count;
	}

	public void ActivatePoint(){
		++activePoints;
		if (activePoints == count) {
			print ("All points activated");
			allActive = true;
			//whatever functionality needs to happen for when all pieces are aligned should go here
			
			UIManager.instance.addDialogue("You have completed the butterfly puzzle.");
		}
	}

	public void DeactivatePoint(){
		--activePoints;
		if (allActive) {
			allActive = false;
		}
	}

	public bool GetAllActive(){
		return allActive;
	}
}
