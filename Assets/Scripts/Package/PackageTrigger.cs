﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PackageTrigger : MonoBehaviour {

	public PackageRB main;

	private bool active = true;

	// Use this for initialization
	void Start () {
		main = transform.parent.GetComponent<PackageRB>();
	}
	
	void OnTriggerStay(Collider other)
	{
		if (!active || other.gameObject != Juanito.ins.JuanitoHuman || !main.active)
			return;

		print ("continuing with active: " + active);

		Vector3 fwd = Juanito.ins.JuanitoHuman.transform.TransformDirection(Vector3.forward);
        Vector3 dir = Vector3.Normalize(transform.position - Juanito.ins.JuanitoHuman.transform.position);
		bool dirCheck = Vector3.Dot(fwd, dir) > 0.5;

		if(dirCheck || main.pushing)
		{
			UIManager.instance.TooltipDisplay("Hold <sprite=0> to Interact");


			if(Input.GetKey(KeyCode.E) || CrossPlatformInputManager.GetButton("Action"))
			{
				UIManager.instance.TooltipDisplay("Use <sprite=4> to Move Package");
				if(!main.pushing)
				{
					main.AttachPlayer();
				}
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
		}

	}

	void OnTriggerExit(Collider other)
	{	
		if(other.gameObject != Juanito.ins.JuanitoHuman || !main.active)
			return;
		
		main.DetachPlayer();
		UIManager.instance.TooltipDisable();

	}

	public void deactivate(){
		print ("deactivating triger");
		active = false;
	}
}
