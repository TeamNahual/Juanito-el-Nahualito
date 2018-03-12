using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ButterflyBody : MonoBehaviour {

	public ButterflyPiece main;
	public int directionFlag;

	// Use this for initialization
	void Start () {
		main = transform.parent.gameObject.GetComponent<ButterflyPiece>();
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject == Juanito.ins.JuanitoHuman && !main.locked)
		{
			if(Input.GetKey(KeyCode.E) || CrossPlatformInputManager.GetButton("Action"))
			{
				if(!main.isPushing)
				{
					main.AttachPlayer();
					main.isPushing = true;
					main.directionFlag = directionFlag;
					main.movementVector = transform.forward;
				}
			}
			else
			{
				if(main.isPushing)
				{
					main.DetachPlayer();
					main.isPushing = false;
				}
			}

		}
	}

	void OnTriggerExit(Collider other) 
	{
		main.DetachPlayer();
		main.isPushing = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
