using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarPiece : MonoBehaviour {

	public bool SeashellRelic;
	public bool TorusRelic;
	public bool NecklaceRelic;

	public Altar main;

	// Use this for initialization
	void Start () {
		main = transform.parent.gameObject.GetComponent<Altar>();

		if(NecklaceRelic)
		{
			GetComponent<Collider>().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
