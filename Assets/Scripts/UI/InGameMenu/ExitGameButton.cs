using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGameButton : MonoBehaviour {
	Button myButton;

	void Awake()
	{
		myButton = GetComponent<Button>();

		myButton.onClick.AddListener(exitGame);
	}

	void exitGame()
	{
		UIManager.instance.exitGame();
	}
}
