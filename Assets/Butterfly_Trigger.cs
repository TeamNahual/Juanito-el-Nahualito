using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly_Trigger : MonoBehaviour {

	//the appropriate difference in angle that is appropriate for rotating the piece
	public float successAngle = 30f;

	//reference to overlapping piece
	private GameObject overlapped = null;
	private butterfly_puzzle_manager manager;
	private bool inPosition = false;

	void Awake(){
		//get reference to manager and increment count of places to put pieces
		manager = GameObject.Find ("Butterfly Puzzle Manager").GetComponent <butterfly_puzzle_manager> ();
		manager.IncreaseCount ();
	}

	void OnTriggerEnter(Collider other){
		print("overlapping");
		if (other.gameObject.tag == "Butterfly Wing") {
			overlapped = other.gameObject;
			StartCoroutine (CheckPosition());
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Butterfly Wing" && overlapped == other.gameObject){
			overlapped = null;
			inPosition = false;
			StopCoroutine (CheckPosition());
		}
	}
		
	//I used a coroutine to check if the piece is in position to reduce runtime
	IEnumerator CheckPosition(){
		print ("Now checking position");
		while (true) {
			float angle = Vector3.Angle( transform.forward, overlapped.transform.forward);
			if (!inPosition && angle <= successAngle) {
				print ("In position");
				manager.ActivatePoint ();
				inPosition = true;
			}else if(inPosition && angle >= successAngle){
				print ("Out of position");
				manager.DeactivatePoint ();
				inPosition = false;
			}
			yield return new WaitForSeconds (0.1f);
		}
	}
}
