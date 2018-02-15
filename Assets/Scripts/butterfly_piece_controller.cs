using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterfly_piece_controller : MonoBehaviour {

	public float moveSpeed;
	public float rotateSpeed;
	public Vector3 puzzleCenter;
	public float centerBuffer;

	private bool moving = false;
	private bool rotating = false;
	private Vector3 direction;
	private int rotateFlag;
	private Transform targetPoint;
	private Transform player;
	private bool locked = false;
	private Vector3 upBoundary;
	private Vector3 downBoundary;
	private Vector3 leftBoundary;
	private Vector3 rightBoundary;

	void Awake(){
		player = GameObject.Find ("ThirdPersonController").transform;

		upBoundary = puzzleCenter + new Vector3 (0f, 0f, centerBuffer);
		downBoundary = puzzleCenter + new Vector3 (0f, 0f, -centerBuffer);
		leftBoundary = puzzleCenter + new Vector3 (-centerBuffer, 0f, 0f);
		rightBoundary = puzzleCenter + new Vector3 (centerBuffer, 0f, 0f);

		if (!CheckBoundaries (transform.position))
			Debug.Log (gameObject.name + " is not within set boundaries on awake");
	}

	//stop all movement on the piece
	public void Stop(){
		moving = false;
		rotating = false;
	}

	//start pushing in designated direction
	public void StartMoving(Vector3 d){
		direction = d;
		moving = true;
	}

	//start rotating either left or right based on flag
	public void StartRotating(int f/*, Transform p*/){
		rotateFlag = f;
		//targetPoint = p;
		rotating = true;
	}

	public void Lock(Quaternion rotation, Vector3 pos){
		locked = true;
		transform.rotation = rotation;
		transform.position = pos;
	}

	private bool CheckBoundaries(Vector3 pos){
		if (pos.z > upBoundary.z) {
			Debug.Log ("Hitting up bounds");
			return false;
		} else if (pos.z < downBoundary.z) {
			Debug.Log ("Hitting down bounds");
			return false;
		} else if (pos.x > rightBoundary.x) {
			Debug.Log ("Hitting right bounds");
			return false;
		} else if (pos.x < leftBoundary.x) {
			Debug.Log ("Hitting left bounds");
			return false;
		} else {
			return true;
		}
	}
		
	void FixedUpdate(){
		if(!locked){
			//adjust piece based on where Juanito is pushing
			if (moving) {
				if(CheckBoundaries(transform.position + direction * Time.deltaTime))
					transform.Translate (direction * Time.deltaTime);
			} else if (rotating) {
				transform.RotateAround (transform.position, Vector3.up, rotateSpeed * Time.deltaTime * rotateFlag);
				//player.LookAt (targetPoint);
			}
		}
	}
}
