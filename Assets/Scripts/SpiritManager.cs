using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;

public class SpiritManager : MonoBehaviour {

	public GameObject Player;
	public GameObject Spirit;

	private float spirit_start_time;
	private float spirit_time_limit = 30;
	private bool SpiritState = false;

	// Use this for initialization
	void Start () {
		Physics.IgnoreCollision(Player.GetComponent<Collider>(), Spirit.GetComponent<Collider>());
	}
	
	// Update is called once per frame
	void Update () {
		SpiritHandler();
	}

 	private void EnterSpiritState()
 	{
 		Player.GetComponent<ThirdPersonUserControl>().enabled = false;
 		Player.GetComponent<ThirdPersonCharacter>().enabled = false;

 		Spirit.transform.position = Player.transform.position;
 		Spirit.SetActive(true);
 		SpiritState = true;

 	}

 	private void SpiritHandler()
 	{
 		if(Input.GetKeyDown(KeyCode.A))
		{
			if(!SpiritState)
			{
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
 		Spirit.SetActive(false);
 		Player.GetComponent<ThirdPersonUserControl>().enabled = true;
 		Player.GetComponent<ThirdPersonCharacter>().enabled = true;
 		SpiritState = false;
 	}
}
