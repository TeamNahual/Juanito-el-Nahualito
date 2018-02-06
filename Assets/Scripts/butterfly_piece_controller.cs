using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterfly_piece_controller : MonoBehaviour {

	public float moveSpeed;
	public float rotateSpeed;

	private bool moving = false;
	private bool rotating = false;
	private Vector3 direction;

	public void Stop(){
		moving = false;
		rotating = false;
	}

	public void StartMoving(Vector3 d){
		direction = d;
		moving = true;
	}

	void FixedUpdate(){
		if (moving) {
			transform.position += (direction * Time.deltaTime);
		} else if (rotating) {
			
		}
	}
}
