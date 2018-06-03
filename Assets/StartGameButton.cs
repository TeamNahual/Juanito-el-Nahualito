using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour {

	public GameObject KeyText;
	private bool state = false;
	private bool interactable = false;

	public bool toAct1 = false;
	public bool toAct2 = true;
	public bool toAct3 = false;

	// Use this for initialization
	void Start () {
		KeyText.SetActive (false);
		StartCoroutine (SetText ());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown && interactable)
		{
			if (!state)
			{
				state = true;
				KeyText.SetActive (true);
			}
			else
			{
				if(toAct1)
					StartAct1();
				else if(toAct2)
					StartScene ();
				else if(toAct3)
					StartAct3();
			}
		}
	}

    public void StartScene()
    {
		GameManager.instance.loadLevel ("l2_zonetest");
    }

	IEnumerator SetText()
	{
		yield return new WaitForSeconds (3f);

		interactable = true;

		yield return new WaitForSeconds (3f);

		state = true;
		KeyText.SetActive (true);

	}

	public void StartAct1()
	{
		GameManager.instance.loadLevel("Lvl 1");
	}

	public void StartAct3()
	{
		GameManager.instance.loadLevel("Lvl 3");
	}
}
