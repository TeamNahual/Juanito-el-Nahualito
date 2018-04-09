using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Butterfly : MonoBehaviour {

	public float DebugRadius = 0;
	public bool tutorial = false;
	public GameObject sphereContainer;

	// Use this for initialization
	void Start () {
		sphereContainer.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(tutorial)
		{
			if(Juanito.ins.SpiritState)
			{
				UIManager.instance.TooltipDisable();
				tutorial = false;
			}
		}
	}

	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Handles.DrawWireDisc(transform.position, Vector3.up, DebugRadius);
	}
	#endif

	void OnTriggerStay(Collider other) 
	{
        if(other.gameObject == Juanito.ins.JuanitoHuman)
        {
	        if(!Juanito.ins.SpiritState)
			{
				if(tutorial)
				{
					UIManager.instance.TooltipDisplay("Press <sprite=3> to Enter Spirit Mode");
				}
			}

			if(Juanito.ins.SpiritState)
			{
				sphereContainer.SetActive(true);
			}
			else
			{
				sphereContainer.SetActive(false);
			}
		}	
	}

	void OnTriggerEnter(Collider other)
	{
		Juanito.ins.inButterflyZone = true;
	}

	void OnTriggerLeave(Collider other)
	{
		Juanito.ins.inButterflyZone = false;
	}
}
