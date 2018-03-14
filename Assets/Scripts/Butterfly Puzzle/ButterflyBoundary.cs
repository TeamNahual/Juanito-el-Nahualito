using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyBoundary : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Physics.IgnoreCollision(Juanito.ins.JuanitoHuman.GetComponent<Collider>(), GetComponent<Collider>());
		Physics.IgnoreCollision(Juanito.ins.JuanitoSpirit.GetComponent<Collider>(), GetComponent<Collider>());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
