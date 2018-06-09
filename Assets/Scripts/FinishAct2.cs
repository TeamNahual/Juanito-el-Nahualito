using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityStandardAssets.Characters.ThirdPerson;

public class FinishAct2 : MonoBehaviour {

	public ThirdPersonUserControl character;

	public PlayableDirector timeline;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Mathf.Round ((float)timeline.time) >= Mathf.Round ((float)timeline.duration))
		{
			UIManager.instance.FadeOut();
			GameManager.instance.loadLevel("Act3Cutscene");
		}
	}

	void OnTriggerEnter(Collider other)
	{
		character.endScene = true;	
	}
}
