using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class Juanito : MonoBehaviour {

	public static Juanito ins;

	public GameObject JuanitoHuman;
	public GameObject JuanitoSpirit;

	private float spirit_start_time;
	private float spirit_time_limit = 50;
	private bool SpiritState = false;

	[HideInInspector]
	public SpiritController SpiritControl;

	private int numberOfButterflies = 0;

	int MAX_SPIRIT_COUNT = 3;

	// Use this for initialization
	void Awake () {
		ins = this;

		Physics.IgnoreCollision(JuanitoHuman.GetComponent<Collider>(), JuanitoSpirit.GetComponent<Collider>());
		SpiritControl = JuanitoSpirit.GetComponent<SpiritController>();
	}
	
	// Update is called once per frame
	void Update () {
		SpiritHandler();
	}

	public int GetSpiritCount()
	{
		return numberOfButterflies;
	}

	public bool AddSpiritCount(int count)
	{
		if(numberOfButterflies < MAX_SPIRIT_COUNT)
		{
			numberOfButterflies += count;
			return true;
		}
		
		return false;
	}

	public void DelSpiritCount(int count)
	{
		numberOfButterflies = Mathf.Max(numberOfButterflies - count, 0); 
	}

	private void EnterSpiritState()
 	{
 		JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = false;
 		JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = false;
 		FancyCam.ins.player = JuanitoSpirit.transform;
 		JuanitoSpirit.transform.position = JuanitoHuman.transform.position;
 		JuanitoSpirit.SetActive(true);
 		SpiritState = true;

 	}

 	private void SpiritHandler()
 	{
 		if(Input.GetKeyDown(KeyCode.Q) || CrossPlatformInputManager.GetButtonDown("Toggle-Spirit"))
		{
			if(!SpiritState && Juanito.ins.GetSpiritCount() >= 3)
			{
				DelSpiritCount(3);
				EnterSpiritState();
				spirit_start_time = Time.time;
			}
			else
			{
				EndSpiritState();
			}
		}

		if(SpiritState && Time.time - spirit_start_time > spirit_time_limit)
		{
			EndSpiritState();
		}
 	}

 	private void EndSpiritState()
 	{
 		SpiritControl.currentFollower = null;
 		JuanitoSpirit.SetActive(false);
 		FancyCam.ins.player = JuanitoHuman.transform;
 		JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = true;
 		JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = true;
 		SpiritState = false;
 	}
}
