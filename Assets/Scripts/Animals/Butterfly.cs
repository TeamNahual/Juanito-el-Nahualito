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
	public GameObject containerLighting;
	public Vector3 initialPosition;

	public bool currState = false;

	private Color mBackgroundColor;

	// Use this for initialization
	void Start () {
		sphereContainer.SetActive(false);
		containerLighting.SetActive(false);
		mBackgroundColor = Camera.main.backgroundColor;
	}
	
	// Update is called once per frame
	void Update () {
		if(tutorial)
		{
			if(Juanito.ins.SpiritState)
			{
				UIManager.instance.TooltipDisable();
				tutorial = false;
				// Juanito.ins.JuanitoHuman.transform.position = initialPosition;
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

			// Debug.Log(Juanito.ins.SpiritState);
			if(Juanito.ins.SpiritState != currState)
			{
				if(Juanito.ins.SpiritState)
				{
					EnterSpiritMode();
				}
				else
				{
					ExitSpiritMode();
				}

				currState = Juanito.ins.SpiritState;
			}
		}	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == Juanito.ins.JuanitoHuman)
        {
			Juanito.ins.inButterflyZone = true;
			Juanito.ins.butterflyZoneOrigin = sphereContainer.transform.position;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject == Juanito.ins.JuanitoHuman)
        {
			Juanito.ins.inButterflyZone = false;
			UIManager.instance.TooltipDisable();
		}
	}

	public void EnterSpiritMode()
	{
		// initialPosition = Juanito.ins.JuanitoHuman.transform.position;
		// Debug.Log(initialPosition);
		Juanito.ins.HumanAnim.SetBool("Kneeling", true);
		//sphereContainer.SetActive(true);
		//containerLighting.SetActive(true);
		//mBackgroundColor = Camera.main.backgroundColor;
		//Camera.main.backgroundColor = new Color(0,0,0,1);
		//LightManager.ins.DisableLights();
		GetComponent<ButterflySpawner>().OnSpiritModeStart();
	}

	public void ExitSpiritMode()
	{
		Juanito.ins.HumanAnim.SetBool("Kneeling", false);
		//sphereContainer.SetActive(false);
		//containerLighting.SetActive(false);
		//Camera.main.backgroundColor = mBackgroundColor;
		//LightManager.ins.EnableLights();
		GetComponent<ButterflySpawner>().OnSpiritModeEnd();
	}
}
