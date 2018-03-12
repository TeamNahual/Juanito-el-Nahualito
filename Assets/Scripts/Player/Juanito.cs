using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class Juanito : MonoBehaviour {

	public static Juanito ins;

	public GameObject JuanitoHuman;
	public GameObject JuanitoSpirit;

	public Animator HumanAnim;
	public Animator SpiritAnim;

	public bool butterflyRelic = false;

	private float spirit_start_time;
	private float spirit_time_limit = 50;
	public bool SpiritState = false;

	[HideInInspector]
	public SpiritController SpiritControl;

	private float numberOfButterflies = 0;

	int MAX_SPIRIT_COUNT = 100;

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

	public float GetSpiritCount()
	{
		return numberOfButterflies;
	}

	public bool AddSpiritCount(float count)
	{
		if(numberOfButterflies < MAX_SPIRIT_COUNT)
		{
			numberOfButterflies += count;
			return true;
		}
		
		return false;
	}

	public void DelSpiritCount(float count)
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
			if(!SpiritState && GetSpiritCount() >= 10)
			{
				EnterSpiritState();
				spirit_start_time = Time.time;
			}
			else
			{
				EndSpiritState();
			}
		}

		if (SpiritState) 
		{
			DelSpiritCount ((Time.deltaTime * 100) / spirit_time_limit);

			if(GetSpiritCount() <= 0)
			{
				EndSpiritState ();
			}
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
