using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ButterflyBody : MonoBehaviour {

	public ButterflyPiece main;
	public int directionFlag;

	int layerMask;

	// Use this for initialization
	void Start () {
		main = transform.parent.gameObject.GetComponent<ButterflyPiece>();

		layerMask = LayerMask.GetMask("ButterflyPiece");
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject == Juanito.ins.JuanitoHuman && !main.locked)
		{
			if(Juanito.ins.CheckFacingObjects(main.meshes, layerMask))
			{
				UIManager.instance.TooltipDisplay("Hold <sprite=0> to Interact");

				if(Input.GetKey(KeyCode.E) || CrossPlatformInputManager.GetButton("Action"))
				{
					UIManager.instance.TooltipDisplay("Use <sprite=4> to Move Piece");
					main.AttachPlayer();
					main.isPushing = true;
					main.directionFlag = directionFlag;
					main.movementVector = transform.forward;
					main.sideMovementVector = transform.right;
				}
				else
				{
					if(main.isPushing)
					{
						UIManager.instance.TooltipDisable();
						main.DetachPlayer();
						main.isPushing = false;
					}
				}
			}
			else
			{
				UIManager.instance.TooltipDisable();
			}
		}
	}

	void OnTriggerExit(Collider other) 
	{
		main.DetachPlayer();
		main.isPushing = false;
		UIManager.instance.TooltipDisable();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
