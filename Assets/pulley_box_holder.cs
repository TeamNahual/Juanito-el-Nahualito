using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulley_box_holder : MonoBehaviour {

	public GameObject lock_object;
	public GameObject release_object;

	private pulley_platform platform;
	private Vector3 lock_position;
	private bool used = false;

	void Awake(){
		platform = GetComponentInParent <pulley_platform> ();
		lock_position = lock_object.transform.position;
	}

	void OnTriggerEnter(Collider other){
		if (!used && other.gameObject.tag == "package") {
			PackageRB p = other.gameObject.GetComponent <PackageRB> ();
			platform.Activate (other.transform, p, release_object.transform);
			p.DetachPlayer ();
			p.gameObject.transform.position = lock_position;
			used = true;
		}
	}
}
