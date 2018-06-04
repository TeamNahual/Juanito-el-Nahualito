using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endlevel1 : MonoBehaviour {

	public bool active = false;

	// Use this for initialization
	void Start () {
		GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == Juanito.ins.JuanitoHuman && active)
		{
			UIManager.instance.FadeOut();
			StartCoroutine(StartLevel());
			active = false;
		}
	}

	IEnumerator StartLevel()
	{
		yield return new WaitForSeconds(2f);
		GameManager.instance.loadLevel("Act2Cutscene");
	}
}
