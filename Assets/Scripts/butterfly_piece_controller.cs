using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterfly_piece_controller : MonoBehaviour {

	public float moveSpeed;
	public float rotateSpeed;
	public float centerBuffer;

	private Vector3 puzzleCenter;
	private bool moving = false;
	private bool rotating = false;
	private Vector3 direction;
	private int rotateFlag;
	private Transform targetPoint;
	// private Transform player;
	private bool locked = false;
	private Vector3 upBoundary;
	private Vector3 downBoundary;
	private Vector3 leftBoundary;
	private Vector3 rightBoundary;
	private Rigidbody rb;

	void Awake(){
		//player = GameObject.Find ("ThirdPersonController").transform;

		puzzleCenter = GameObject.Find ("Butterfly Puzzle Center").transform.position;

		rb = GetComponent<Rigidbody>();

		upBoundary = puzzleCenter + new Vector3 (0f, 0f, centerBuffer);
		downBoundary = puzzleCenter + new Vector3 (0f, 0f, -centerBuffer);
		leftBoundary = puzzleCenter + new Vector3 (-centerBuffer, 0f, 0f);
		rightBoundary = puzzleCenter + new Vector3 (centerBuffer, 0f, 0f);

		/*upBoundary = transform.position + new Vector3 (0f, 0f, centerBuffer);
		downBoundary = transform.position + new Vector3 (0f, 0f, -centerBuffer);
		leftBoundary = transform.position + new Vector3 (-centerBuffer, 0f, 0f);
		rightBoundary = transform.position + new Vector3 (centerBuffer, 0f, 0f);*/

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
		if(!locked){
			//adjust piece based on where Juanito is pushing
			if (moving) {
				rb.velocity = direction * moveSpeed;
			} else if (rotating) {
				transform.RotateAround (transform.position, Vector3.up, rotateSpeed * Time.deltaTime * rotateFlag);
				//player.LookAt (targetPoint);
			} else {
				rb.velocity = Vector3.zero;
			}
			CheckBoundaries (transform.position);
		}
	}
}
