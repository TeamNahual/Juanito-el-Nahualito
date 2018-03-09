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
			if(Input.GetKeyDown(KeyCode.E))
			{
				if(main.isPushing)
				{
					main.DetachPlayer();
					main.isPushing = false;
				}
				else
				{
					main.AttachPlayer();
					main.isPushing = true;
					main.directionFlag = directionFlag;
					main.movementVector = transform.right;
				}
			}

		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.E) && main.isPushing)
		{
			main.DetachPlayer();
			main.isPushing = false;
		}
	}
}
