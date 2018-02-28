using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Butterfly : MonoBehaviour {

	Renderer rend;

	public float DebugRadius = 0;

	float timeout = 3;
	float timeout_start;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
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
//				rend.enabled = false;
//				isActive = false;
//				timeout_start = Time.time;
			}
		}
	}
}
