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
				EnterSpiritMode();
			}
			else
			{
				ExitSpiritMode();
			}
		}	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == Juanito.ins.JuanitoHuman)
        {
			Juanito.ins.inButterflyZone = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject == Juanito.ins.JuanitoHuman)
        {
			Juanito.ins.inButterflyZone = false;
		}
	}

	public void EnterSpiritMode()
	{
		Juanito.ins.HumanAnim.SetBool("Kneeling", true);
		sphereContainer.SetActive(true);
		containerLighting.SetActive(true);
		Camera.main.backgroundColor = new Color(0,0,0,1);
		LightManager.ins.DisableLights();
	}

	public void ExitSpiritMode()
	{
		Juanito.ins.HumanAnim.SetBool("Kneeling", false);
		sphereContainer.SetActive(false);
		containerLighting.SetActive(false);
		Camera.main.backgroundColor = mBackgroundColor;
		LightManager.ins.EnableLights();
	}
}
