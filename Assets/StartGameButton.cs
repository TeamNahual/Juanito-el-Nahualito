using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour {

	public GameObject KeyText;
	private bool state = false;
	private bool interactable = false;

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
				StartScene ();
		}
	}

    public void StartScene()
    {
		GameManager.instance.loadLevel ("Level 2 FinalSlice");
    }

	IEnumerator SetText()
	{
		yield return new WaitForSeconds (3f);

		interactable = true;

		yield return new WaitForSeconds (3f);

		state = true;
		KeyText.SetActive (true);

	}
}
