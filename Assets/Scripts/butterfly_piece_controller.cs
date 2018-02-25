using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterfly_piece_controller : MonoBehaviour {

	public float moveSpeed;
	public float rotateSpeed;
	public float centerBuffer;

	private int rotateFlag;

	private Transform targetPoint;

	private bool locked = false;
	private bool moving = false;
	private bool rotating = false;
	private bool colliding = false;

	private Vector3 puzzleCenter;
	private Vector3 direction;
	private Vector3 upBoundary;
	private Vector3 downBoundary;
	private Vector3 leftBoundary;
	private Vector3 rightBoundary;

	private Rigidbody rb;

	void Awake(){
		puzzleCenter = GameObject.Find ("Butterfly Puzzle Center").transform.position;

		rb = GetComponent<Rigidbody>();

		upBoundary = puzzleCenter + new Vector3 (0f, 0f, centerBuffer);
		downBoundary = puzzleCenter + new Vector3 (0f, 0f, -centerBuffer);
		leftBoundary = puzzleCenter + new Vector3 (-centerBuffer, 0f, 0f);
		rightBoundary = puzzleCenter + new Vector3 (centerBuffer, 0f, 0f);

		if (!CheckBoundaries (transform.position))
			Debug.Log (gameObject.name + " is not within set boundaries on awake\nPuzzle center: "+puzzleCenter+"\nGame object position: "+transform.position+"\nBuffer: "+centerBuffer);
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
	public void StartRotating(int f){
		rotateFlag = f;
		rotating = true;
	}

	public void Lock(Quaternion rotation, Vector3 pos){
		locked = true;
		transform.rotation = rotation;
		transform.RotateAround (transform.position, transform.right, 90f);
		transform.position = new Vector3(pos.x, pos.y + 0.75f, pos.z);
		rb.constraints = RigidbodyConstraints.FreezeAll;
	}

	private bool CheckBoundaries(Vector3 pos){
		bool flag = true;
		if (pos.z > upBoundary.z) {
			Debug.Log ("Hitting up bounds\nBounds: "+upBoundary.z+"\nnew pos: "+pos.z);
			transform.position = new Vector3 (transform.position.x, transform.position.y, upBoundary.z);
			flag = false;
		}
		if (pos.z < downBoundary.z) {
			Debug.Log ("Hitting down bounds\nBounds: "+downBoundary.z+"\nnew pos: "+pos.z);
			transform.position = new Vector3 (transform.position.x, transform.position.y, downBoundary.z);
			flag = false;
		}
		if (pos.x > rightBoundary.x) {
			Debug.Log ("Hitting right bounds\nBounds: "+rightBoundary.x+"\nnew pos: "+pos.x);
			transform.position = new Vector3 (rightBoundary.x, transform.position.y, transform.position.z);
			flag = false;
		}
		if (pos.x < leftBoundary.x) {
			Debug.Log ("Hitting left bounds\nBounds: "+leftBoundary.x+"\nnew pos: "+pos.x);
			transform.position = new Vector3 (leftBoundary.x, transform.position.y, transform.position.z);
			flag = false;
		}
		return flag;
	}
		
	void FixedUpdate(){
		if (!locked) {
			//adjust piece based on where Juanito is pushing
			rb.mass = 20;
			if (moving) {
				rb.velocity = direction * moveSpeed;
			} else if (rotating) {
				transform.RotateAround (transform.position, Vector3.up, rotateSpeed * Time.deltaTime * rotateFlag);
			} else {
				rb.velocity = Vector3.zero;
				rb.mass = 1000;
			}
			CheckBoundaries (transform.position);
		} 
	}

}
