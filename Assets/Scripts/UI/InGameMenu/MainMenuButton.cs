using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour {
	Button myButton;

	void Awake()
	{
		myButton = GetComponent<Button>();

		myButton.onClick.AddListener(toMain);
	}

	void toMain()
	{
		if (GameManager.instance != null)
		{
			SceneManager.LoadScene("MainMenu");
		}
	}
}
