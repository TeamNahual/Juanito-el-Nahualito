using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulley_platform : MonoBehaviour {

	private float weight = 0f;
	private Dictionary<string, float> weights = new Dictionary<string, float>{ {"medicine", 1f} };

	// Use this for initialization
	void Start () {
		
	}
	
	public float GetWeight(){
		return weight;
	}
}
