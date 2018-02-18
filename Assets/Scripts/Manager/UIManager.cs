using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class UIManager : MonoBehaviour
{
	public static UIManager instance = null; // Allows us to access this from other scripts
	
	// Menu Related
	public GameObject inGameMenu;
	private bool menuToggleProtect;
    private bool isMenuOpen;
	
	// Dialogue System
	public GameObject dialogueUI;
	public DialogueSystem dialogueSystem;
	private bool dialogueToggleProtect;
    private bool isDialogueOpen;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		isMenuOpen = false;
		menuToggleProtect = false;
		
		dialogueSystem = new DialogueSystem();
		dialogueToggleProtect = false;
		isDialogueOpen = false;
	}
	
    void Start()
    {
		DynamicGI.UpdateEnvironment(); // Get rid of this eventually
	}
	
	void Update()
	{
		if (Input.GetKey(KeyCode.Escape)) {
			if (!menuToggleProtect) {
				isMenuOpen = !isMenuOpen;
			}
			menuToggleProtect = true;
		} else if (menuToggleProtect) {
			menuToggleProtect = false;
		}
		
		if (Input.GetKey(KeyCode.Return) || CrossPlatformInputManager.GetButtonDown("Dialogue-Pop")) {
			if (!dialogueToggleProtect) {
				dialogueSystem.doDialogueAction();
			}
			dialogueToggleProtect = true;
		} else if (dialogueToggleProtect) {
			dialogueToggleProtect = false;
		}
		dialogueSystem.Update();
		
		inGameMenu.gameObject.SetActive(isMenuOpen);
		dialogueUI.gameObject.SetActive(isDialogueOpen);
	}
	
	public void closeMainMenu()
	{
		isMenuOpen = false;
	}
	
	public void toggleDialogueBox(bool toggle) {
		isDialogueOpen = toggle;
	}
	
	public void setDialogueBoxText(string text) {
		dialogueUI.transform.Find("Dialogue").GetComponent<Text>().text = text;
	}
}
