using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

	public float rotationsPerMinute = 10.0f;
	
	void Update()
	{
		transform.Rotate(Vector3.up * rotationsPerMinute *Time.deltaTime, Space.World);
	}
}
