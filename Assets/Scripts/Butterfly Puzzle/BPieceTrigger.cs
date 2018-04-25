using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class BPieceTrigger : MonoBehaviour {

	private ButterflyBodyV2 main;

	void Awake()
	{
		main = transform.parent.GetComponent<ButterflyBodyV2>();
	}

	void OnTriggerStay(Collider other)
	{
		if(main.locked) return;
		
		if(Juanito.ins.CheckFacingObjectsSpirit(main.allObjects, main.layerMask))
		{
			UIManager.instance.TooltipDisplay("Hold <sprite=0> to Interact");


			if(Input.GetKey(KeyCode.E) || CrossPlatformInputManager.GetButton("Action"))
			{
				if(!main.pushing)
					main.AttachPlayer();
			}
			else
			{
				if(main.pushing)
					main.DetachPlayer();
			}
		}
		else
		{
			UIManager.instance.TooltipDisable();
			// main.DetachPlayer();
		}
	}
}
