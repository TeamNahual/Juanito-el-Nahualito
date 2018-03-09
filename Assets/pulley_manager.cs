using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulley_manager : MonoBehaviour {

	public Transform top, mid, bottom;
	public pulley_platform plat1, plat2;

	private Vector3 topPos, midPos, bottomPos;

	// Use this for initialization
	void Start () {
		topPos = top.position;
		midPos = mid.position;
		bottomPos = bottom.position;
	}
	
	public void MovePlatforms(){
		
	}
}
