using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterfly_piece_controller : MonoBehaviour {

	public float moveSpeed;
	public float rotateSpeed;

	private bool moving = false;
	private bool rotating = false;
	private Vector3 direction;
	private int rotateFlag;

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
		
	void FixedUpdate(){
		//adjust piece based on where Juanito is pushing
		if (moving) {
			transform.Translate (direction * Time.deltaTime);
		} else if (rotating) {
			transform.RotateAround (transform.position, Vector3.up, rotateSpeed * Time.deltaTime * rotateFlag);
		}
	}
}
