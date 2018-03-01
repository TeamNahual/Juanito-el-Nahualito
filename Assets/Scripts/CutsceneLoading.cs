using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneLoading : MonoBehaviour {

	public Text LoadingText;
	public Text ProgressText;
	public Text ContinueText;

	private AsyncOperation ao;

	// Use this for initialization
	void Start () {
		ContinueText.gameObject.SetActive (false);
		StartCoroutine (LoadNextLevel ());
	}
	
	// Update is called once per frame
	void Update () {
		if (ContinueText.gameObject.activeSelf)
		{
			if (Input.anyKeyDown)
				ao.allowSceneActivation = true;
		}
	}

	IEnumerator LoadNextLevel()
	{
		yield return new WaitForSeconds (1.5f);

		Application.backgroundLoadingPriority = ThreadPriority.Low;

		ao = SceneManager.LoadSceneAsync (1);

		while (!ao.isDone) 
		{
			if (ao.progress != 0.9f)
				ProgressText.text = Mathf.RoundToInt ((ao.progress / 0.9f) * 100) + "%";

			yield return null;

		}
	}
}
