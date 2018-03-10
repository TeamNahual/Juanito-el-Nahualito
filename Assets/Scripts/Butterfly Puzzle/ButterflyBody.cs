using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyBody : MonoBehaviour {

	public ButterflyPiece main;
	public int directionFlag;

	// Use this for initialization
	void Start () {
		main = transform.parent.gameObject.GetComponent<ButterflyPiece>();
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject == Juanito.ins.JuanitoHuman)
		{
			if(Input.GetKey(KeyCode.E))
			{
				if(!main.isPushing)
				{
					main.AttachPlayer();
					main.isPushing = true;
					main.directionFlag = directionFlag;
					main.movementVector = transform.right;
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
	
	// Update is called once per frame
	void Update () {
		
	}
}
