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
	private int controlIndex = 0;
	private GameObject[] buttons;
	private bool controllerDown = false;
	[SerializeField]
	private GameObject KeyboardButton, ControllerButton, ControlPanel;
	[SerializeField]
	private Sprite defaultButton, pressedButton;


	// Dialogue System
	public GameObject dialogueUI;
	public DialogueSystem dialogueSystem;
	private bool dialogueToggleProtect;
    private bool isDialogueOpen;
	public AudioSource dialogueAudioSource;

	// Action Messages
	public TextMeshProUGUI tooltip; 

	//Audio Snapshots When Pausing
	public AudioMixerSnapshot paused;
	public AudioMixerSnapshot unpaused;

	//Fade Manager
	public Image fadeImage;
	public float fadeDuration = 2f;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		isMenuOpen = false;
		menuToggleProtect = false;
		menuOptionProtect = false;
		tooltip.gameObject.SetActive(false);
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
		FadeIn(fadeDuration);
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
		else
		{
			
		}

		if( Input.GetAxis("Mouse X") != 0 &&  Input.GetAxis("Mouse Y") != 0)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			EventSystem.current.SetSelectedGameObject(null);
		}

	}

	public void FadeIn(float duration = 1)
	{
		StartCoroutine(FadeInRoutine(duration));
	}

	IEnumerator FadeInRoutine(float duration)
	{
		Color color = fadeImage.color;

		float k = 0;

		while(k < duration)
		{
			color.a = 1 - k/duration;

			fadeImage.color = color;

			k += Time.deltaTime;
			yield return null;
		}

		color.a = 0;

		fadeImage.color = color;
	}

	public void FadeOut()
	{

	}

	void FixedUpdate()
	{
		//Debug.Log ("Is the Menu Open: " + isMenuOpen);
		if (isMenuOpen) {
			paused.TransitionTo (.01f);
		}
		if (!isMenuOpen) {
			unpaused.TransitionTo (.01f);
		}
		if(controlsOpen)
		{
			if(CrossPlatformInputManager.GetButtonDown("Menu-LB"))
			{
				switchControls(0);
			}

			if(CrossPlatformInputManager.GetButtonDown("Menu-RB"))
			{
				switchControls(1);
			}
		}



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
					CloseControls();
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
			//SceneManager.LoadScene("MainMenu");
			GameManager.instance.loadLevel("MainMenu");
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

		if(CheckControllers())
		{
			controlIndex = 1;
			controlMenu.transform.GetChild(0).gameObject.SetActive(true);
		}
		else
		{
			controlIndex = 0;
			controlMenu.transform.GetChild(0).gameObject.SetActive(false);
		}

		SetControlIndex();
	}

	public void switchControls(int index)
	{
		controlIndex = index;
		SetControlIndex();
	}

	void SetControlIndex()
	{
		ControlPanel.transform.GetChild(0).gameObject.SetActive(false);
		ControlPanel.transform.GetChild(1).gameObject.SetActive(false);
		ControlPanel.transform.GetChild(controlIndex).gameObject.SetActive(true);

		if(controlIndex == 0)
		{
			KeyboardButton.GetComponent<Image>().sprite = pressedButton;
			ControllerButton.GetComponent<Image>().sprite = defaultButton;
		}
		else
		{
			KeyboardButton.GetComponent<Image>().sprite = defaultButton;
			ControllerButton.GetComponent<Image>().sprite = pressedButton;
		}
	}

	public void pauseMixer(){
		if (Time.timeScale == 0){
			paused.TransitionTo (.25f);
		}
		else {
			unpaused.TransitionTo(.25f);
		}
	}
}
