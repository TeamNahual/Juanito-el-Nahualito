using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneLoading : MonoBehaviour {

	public Text LoadingText;
	public Text ProgressText;
	public Image logo;

	public float rate = 1;
	public float logoAlpha = 255;
	private bool reverse = true;

	private AsyncOperation ao;

	// Use this for initialization
	void Start () {
		//StartCoroutine (LoadNextLevel ());
	}
	
	// Update is called once per frame
	void Update () {

		logoAlpha = (reverse ? logoAlpha - rate : logoAlpha + rate);
			
		if(logoAlpha < 0)
		{
			logoAlpha = 0;
			reverse = false;
		}

		if(logoAlpha > 255)
		{
			logoAlpha = 255;
			reverse = true;
		}

		logo.color = new Color(1f,1f,1f, logoAlpha/255);
	}

	IEnumerator LoadNextLevel()
	{
		yield return new WaitForSeconds (1.5f);

		Application.backgroundLoadingPriority = ThreadPriority.Low;

		ao = SceneManager.LoadSceneAsync (GameManager.instance.levelToLoad);

		while (!ao.isDone) 
		{
			if (ao.progress < 0.9f)
				ProgressText.text = Mathf.RoundToInt ((ao.progress / 0.9f) * 100) + "%";
			else
				ProgressText.text = "100%";

			yield return null;

		}
	}
}
