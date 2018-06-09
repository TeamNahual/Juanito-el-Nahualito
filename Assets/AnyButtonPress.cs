using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoozyUI;

public class AnyButtonPress : MonoBehaviour {

	public GameObject TitleButton;

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.anyKeyDown)
		{
			TitleButton.GetComponent<UIButton>().ExecuteClick();
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(Cursor.visible)
			{
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
			else
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
		}
	}
}
