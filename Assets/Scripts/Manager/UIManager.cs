using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Audio;
 using UnityEngine.EventSystems;
using TMPro;

public class UIManager : MonoBehaviour
{
	public static UIManager instance = null; // Allows us to access this from other scripts
	
	// Menu Related
	public GameObject inGameMenu;
	public GameObject controlMenu;
	private bool menuToggleProtect;
    private bool isMenuOpen;
	private bool menuOptionProtect;
	private bool controlsOpen;

	// Button Related
	public GameObject buttonContinue;
	public GameObject buttonControls;
	public GameObject buttonMainMenu;
	public GameObject buttonExit;
	public GameObject buttonCloseControls;

	// Controller Related
	private int buttonIndex = 0;
	private GameObject[] buttons;
	private bool controllerDown = false;
	
	// Dialogue System
	public GameObject dialogueUI;
	public DialogueSystem dialogueSystem;
	private bool dialogueToggleProtect;
    private bool isDialogueOpen;
	public AudioSource dialogueAudioSource;
	
	// Butterflies
	public GameObject butterflyUI;
	private Color uiColor;

	// Action Messages
	public GameObject pushHelp;
	public GameObject pushMoveHelp;
	public TextMeshProUGUI tooltip; 

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		isMenuOpen = false;
		menuToggleProtect = false;
		menuOptionProtect = false;
		pushHelp.SetActive(false);
		pushMoveHelp.SetActive(false);
		tooltip.gameObject.SetActive(false);
		uiColor = butterflyUI.GetComponent<Image>().color;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		dialogueSystem = new DialogueSystem();
		dialogueToggleProtect = false;
		isDialogueOpen = false;
		buttons = new GameObject[] {buttonContinue, buttonControls, buttonMainMenu, buttonExit};
		controlsOpen = false;
		controlMenu.SetActive(false);
	}
	
    void Start()
    {
		DynamicGI.UpdateEnvironment(); // Get rid of this eventually
	}
	
	public void CloseControls()
	{
		controlsOpen = false;
		controlMenu.SetActive(false);

		EventSystem.current.SetSelectedGameObject(null);
		buttonIndex = 1;
		EventSystem.current.SetSelectedGameObject(buttons[buttonIndex]);
	}


	IEnumerator SelectGameObjectLater(GameObject objectToSelect)
	{
		yield return null;
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(objectToSelect);
	}

	public void HandleControllerNavigation()
	{
		if(!CheckControllers())
			return;

		if(!controlsOpen)
		{
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
		}

		if( Input.GetAxis("Mouse X") != 0 &&  Input.GetAxis("Mouse Y") != 0)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			EventSystem.current.SetSelectedGameObject(null);
		}

	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Escape) || CrossPlatformInputManager.GetButtonDown("Menu-Toggle")) 
		{
			if (!menuToggleProtect) 
			{
				isMenuOpen = !isMenuOpen;
				if (isMenuOpen) 
				{
					if(CheckControllers())
					{
						buttonContinue.GetComponent<Button>().Select();
						StartCoroutine(SelectGameObjectLater(buttonContinue));
					}
					else
					{
						Cursor.visible = true;
						Cursor.lockState = CursorLockMode.None;
					}
					GameManager.instance.lockMovement();
				} 
				else 
				{
					GameManager.instance.unlockMovement();
					Cursor.visible = false;
					Cursor.lockState = CursorLockMode.Locked;
				}
			}
			menuToggleProtect = true;
		} 
		else if (menuToggleProtect) 
		{
			menuToggleProtect = false;
		}
		
		if (isMenuOpen) {
			HandleControllerNavigation();
			if (CrossPlatformInputManager.GetButtonDown("Dialogue-Pop")) 
			{
				// if (!menuOptionProtect) 
				// {
				// 	reloadScene();
				// }
				// menuOptionProtect = true;
			} 
			else if (CrossPlatformInputManager.GetButtonDown("Exit-Game")) 
			{
				if (!menuOptionProtect) 
				{
					exitGame();
				}
				menuOptionProtect = true;
			} 
			else if (menuOptionProtect) 
			{
				menuOptionProtect = false;
			}
		} 
		else 
		{
			if (Input.GetKey(KeyCode.Return) || CrossPlatformInputManager.GetButtonDown("Dialogue-Pop")) 
			{
				if (!dialogueToggleProtect) 
				{
					dialogueSystem.doDialogueAction();
				}
				dialogueToggleProtect = true;
			} 
			else if (dialogueToggleProtect) 
			{
				dialogueToggleProtect = false;
			}
			dialogueSystem.Update();
		}
		
		// Update butteflies
		updateButterflyUI(Juanito.ins.GetSpiritCount());
		
		inGameMenu.gameObject.SetActive(isMenuOpen);
		dialogueUI.gameObject.SetActive(isDialogueOpen);
	}
	
	public void TooltipDisplay(string tip)
	{
		tooltip.gameObject.SetActive(true);
		tooltip.text = tip;
	}

	public void TooltipDisable()
	{
		tooltip.gameObject.SetActive(false);
	}

	public void TooltipInteract()
	{
		tooltip.gameObject.SetActive(true);
		tooltip.text = "Press <sprite=0> to Interact";
	}

	public void closeMainMenu()
	{
		if(controlsOpen)
		{
			CloseControls();
			return;
		}

		isMenuOpen = false;
		GameManager.instance.unlockMovement();
	}
	
	// public void reloadScene() {
	// 	if (GameManager.instance != null)
	// 	{
	// 		GameManager.instance.reloadScene();
	// 	}
	// }

	public void toMainMenu()
	{
		if (GameManager.instance != null)
		{
			SceneManager.LoadScene("MainMenu");
		}
	}
	
	public void exitGame() {
		if (GameManager.instance != null)
		{
			GameManager.instance.exitGame();
		}
	}
	
	public void toggleDialogueBox(bool toggle) {
		isDialogueOpen = toggle;
	}
	
	public void setDialogueBoxText(string text) {
		dialogueUI.transform.Find("Dialogue").GetComponent<Text>().text = text;
	}
	
	public void setAndPlayAudioClip(AudioClip clip) {
		dialogueAudioSource.clip = clip;
		dialogueAudioSource.Play();
	}
	
	private void updateButterflyUI(float count) {
		//string text = (count < 100)? "Butterflies: " + Mathf.RoundToInt(count) + " / 100": "Spirit form ready!";

		if(count < 10)
		{
			butterflyUI.GetComponent<Image>().color = new Color(1,0,0,1);
		}
		else
		{
			butterflyUI.GetComponent<Image>().color = uiColor;
		}

		butterflyUI.GetComponent<Image>().fillAmount = count/100;
	}

	public bool CheckControllers()
	{
		string[] controllers = Input.GetJoystickNames();

		if(controllers.Length == 0)
		{
			return false;
		}

		return true;

	}

	public void OpenControls()
	{
		controlsOpen = true;
		controlMenu.SetActive(true);
		StartCoroutine(SelectGameObjectLater(buttonCloseControls));
	}
}
