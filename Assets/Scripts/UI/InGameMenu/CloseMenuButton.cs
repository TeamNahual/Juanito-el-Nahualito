﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseMenuButton : MonoBehaviour {
	Button myButton;

	void Awake()
	{
		myButton = GetComponent<Button>();

		myButton.onClick.AddListener(closeMenu);
	}

	void closeMenu()
	{
		if (GameManager.instance != null)
		{
			GameManager.instance.closeMainMenu();
		}
	}
}