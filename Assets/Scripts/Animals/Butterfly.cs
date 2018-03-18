using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Butterfly : MonoBehaviour {

	public float DebugRadius = 0;
	public bool tutorial = false;

	// Use this for initialization
	void Start () {

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

	void OnTriggerStay(Collider other) {
		// Debug.Log(other.gameObject);

        if(other.gameObject == Juanito.ins.JuanitoHuman && !Juanito.ins.SpiritState)
		{
			if(Juanito.ins.AddSpiritCount(Time.deltaTime * 10))
			{
				// Do Something if able to add spirits

				if(tutorial)
				{
					if(Juanito.ins.GetSpiritCount() > 30)
					{
						UIManager.instance.TooltipDisplay("Press <sprite=3> to Enter Spirit Mode");
					}
				}
			}
		}
	}
}
