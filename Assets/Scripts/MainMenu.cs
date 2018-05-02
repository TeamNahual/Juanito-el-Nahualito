using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

	public GameObject[] buttons;
	private int buttonIndex = 0;
	private bool controllerDown = false;

	// Use this for initialization
	void Start () {
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(buttons[0]);
	}
	
	// Update is called once per frame
	void Update () {
		HandleControllerNavigation();
	}

	IEnumerator SelectFirst()
	{
		yield return null;
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(buttons[0]);
	}

	public void HandleControllerNavigation()
	{
		string[] controllers = Input.GetJoystickNames();

		if(controllers.Length == 0)
		{
			return;
		}

		float v = CrossPlatformInputManager.GetAxis("Vertical-Joystick");

		if(!controllerDown)
		{
			if(v >= 0.4f)
			{
				buttonIndex = (buttonIndex - 1 < 0 ? 0 : buttonIndex - 1);
				EventSystem.current.SetSelectedGameObject(null);
				EventSystem.current.SetSelectedGameObject(buttons[buttonIndex]);
				controllerDown = true;
			}
			else if(v <= -0.4f)
			{
				buttonIndex = (buttonIndex + 1 >= buttons.Length ? buttons.Length - 1 : buttonIndex + 1);
				EventSystem.current.SetSelectedGameObject(null);
				EventSystem.current.SetSelectedGameObject(buttons[buttonIndex]);
				controllerDown = true;
			}
		}
		
		if(v == 0)
		{
			controllerDown = false;
		}


		if( Input.GetAxis("Mouse X") != 0 &&  Input.GetAxis("Mouse Y") != 0)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			EventSystem.current.SetSelectedGameObject(null);
		}

	}

	public void ToAct2()
	{
		GameManager.instance.loadLevel ("Act2Cutscene");
		// GameManager.instance.loadLevel("l2_zonetest");
	}

	public void ToAct1()
	{
		GameManager.instance.loadLevel("Act 1");
	}

	public void ToAct3()
	{
		GameManager.instance.loadLevel("FlightTest");
	}

	public void EndGame()
	{
		Application.Quit();
	}
}
