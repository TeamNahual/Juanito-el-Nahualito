using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulley_platform : MonoBehaviour {

	//public BoxCollider stationaryCollider;
	public float start_weight = 0f;

	private float weight;
	private pulley_manager manager;
	private bool moving = false;
	private Vector3 target;
	private Vector3 direction;
	private Dictionary<string, float> weights = new Dictionary<string, float>{ {"medicine", 1f}, {"dog", 3f} };

	void Awake(){
		manager = GetComponentInParent <pulley_manager> ();
		weight = start_weight;
	}
	
	public float GetWeight(){
		return weight;
	}

	public Vector3 GetPosition(){
		return transform.parent.position;
	}

	public void MoveTo(Vector3 pos){
		
		if (pos.y < transform.position.y) {
			print ("going down to " + pos.y);
			target = pos;
			moving = true;
			direction = -Vector3.up;
		} else if (pos.y > transform.position.y) {
			print ("going up to " + pos.y);
			target = pos;
			moving = true;
			direction = Vector3.up;
		}
	}

	void OnTriggerEnter(Collider other){
        Debug.Log("2");
		if (other.GetComponent<package> ())
        {
			weight += weights ["medicine"];
			other.gameObject.transform.parent = transform.parent;
		}
        else
        {
            weight += weights["dog"];
        }
	}

	void OnTriggerExit(Collider other){
		if (other.GetComponent<package> ())
        {
			weight -= weights ["medicine"];
			other.gameObject.transform.parent = null;
		}
        else
        {
            weight -= weights["dog"];
        }
	}

	void Update(){
		if (moving) {
			transform.parent.Translate (direction * Time.deltaTime);
			float posY = transform.parent.position.y;
			if (Mathf.Abs (posY - target.y) < 0.1f) {
				transform.parent.position = new Vector3(transform.parent.position.x, target.y, transform.parent.position.z);
				moving = false;
			}
		}
	}
		
}
