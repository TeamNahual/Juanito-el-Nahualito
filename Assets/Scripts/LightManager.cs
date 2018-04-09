using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour {

	public static LightManager ins;

	public GameObject[] lightsInScene;

	// Use this for initialization
	void Start () {
		ins = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DisableLights()
	{
		foreach(GameObject light in lightsInScene)
		{
			light.SetActive(false);
		}
	}

	public void EnableLights()
	{
		foreach(GameObject light in lightsInScene)
		{
			light.SetActive(true);
		}
	}
}
