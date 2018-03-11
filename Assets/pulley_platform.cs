using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulley_platform : MonoBehaviour {

	private float weight = 0f;
	private pulley_manager manager;
	private bool moving = false;
	private Vector3 target;
	private Vector3 direction;
	private Dictionary<string, float> weights = new Dictionary<string, float>{ {"medicine", 1f} };

	void Awake(){
		manager = GetComponentInParent <pulley_manager> ();
	}
	
	public float GetWeight(){
		return weight;
	}

	public void MoveTo(Vector3 pos){
		if (pos.y < transform.position.y) {
			print ("going down");
			target = pos;
			moving = true;
			direction = -Vector3.up;
		} else if (pos.y > transform.position.y) {
			print ("going up");
			target = pos;
			moving = true;
			direction = Vector3.up;
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.GetComponent<package> ()) {
			weight += weights ["medicine"];
			manager.MovePlatforms ();
			other.gameObject.transform.parent = transform.parent;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.GetComponent<package> ()) {
			weight -= weights ["medicine"];
			manager.MovePlatforms ();
			other.gameObject.transform.parent = null;
		}
	}

	void Update(){
		if (moving) {
			transform.parent.Translate (direction * Time.deltaTime);
			float posY = transform.parent.position.y;
			float targetY = target.y;
			if (Mathf.Abs (posY - targetY) < 0.1f) {
				transform.parent.position = new Vector3(transform.parent.position.x, targetY, transform.parent.position.z);
				moving = false;
			}
		}
	}
		
}
