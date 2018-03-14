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
				UIManager.instance.pushHelp.SetActive(true);

				if(Input.GetKey(KeyCode.E) || CrossPlatformInputManager.GetButton("Action"))
				{
					UIManager.instance.pushHelp.SetActive(false);
					UIManager.instance.pushMoveHelp.SetActive(true);
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
						UIManager.instance.pushMoveHelp.SetActive(false);
						main.DetachPlayer();
						main.isPushing = false;
					}
				}
			}
			else
			{
				UIManager.instance.pushHelp.SetActive(false);
			}
		}
	}

	void OnTriggerExit(Collider other) 
	{
		main.DetachPlayer();
		main.isPushing = false;
		UIManager.instance.pushHelp.SetActive(false);
		UIManager.instance.pushMoveHelp.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
