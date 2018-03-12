using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
	public static UIManager instance = null; // Allows us to access this from other scripts
	
	// Menu Related
	public GameObject inGameMenu;
	private bool menuToggleProtect;
    private bool isMenuOpen;
	private bool menuOptionProtect;
	
	// Dialogue System
	public GameObject dialogueUI;
	public DialogueSystem dialogueSystem;
	private bool dialogueToggleProtect;
    private bool isDialogueOpen;
	public AudioSource dialogueAudioSource;
	
	// Butterflies
	public GameObject butterflyUI;

	// Action Messages
	public GameObject pushHelp;
	public GameObject pushMoveHelp;

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
		if (Input.GetKey(KeyCode.Escape) || CrossPlatformInputManager.GetButtonDown("Menu-Toggle")) {
			if (!menuToggleProtect) {
				isMenuOpen = !isMenuOpen;
				if (isMenuOpen) {
					GameManager.instance.lockMovement();
				} else {
					GameManager.instance.unlockMovement();
				}
			}
			menuToggleProtect = true;
		} else if (menuToggleProtect) {
			menuToggleProtect = false;
		}
		
		if (isMenuOpen) {
			if (CrossPlatformInputManager.GetButtonDown("Dialogue-Pop")) {
				if (!menuOptionProtect) {
					reloadScene();
				}
				menuOptionProtect = true;
			} else if (CrossPlatformInputManager.GetButtonDown("Exit-Game")) {
				if (!menuOptionProtect) {
					exitGame();
				}
				menuOptionProtect = true;
			} else if (menuOptionProtect) {
				menuOptionProtect = false;
			}
		} else {
			if (Input.GetKey(KeyCode.Return) || CrossPlatformInputManager.GetButtonDown("Dialogue-Pop")) {
				if (!dialogueToggleProtect) {
					dialogueSystem.doDialogueAction();
				}
				dialogueToggleProtect = true;
			} else if (dialogueToggleProtect) {
				dialogueToggleProtect = false;
			}
			dialogueSystem.Update();
		}
		
		// Update butteflies
		updateButterflyUI(Juanito.ins.GetSpiritCount());
		
		inGameMenu.gameObject.SetActive(isMenuOpen);
		dialogueUI.gameObject.SetActive(isDialogueOpen);
	}
	
	public void closeMainMenu()
	{
		isMenuOpen = false;
		GameManager.instance.unlockMovement();
	}
	
	public void reloadScene() {
		if (GameManager.instance != null)
		{
			GameManager.instance.reloadScene();
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
		string text = (count < 100)? "Butterflies: " + Mathf.RoundToInt(count) + " / 100": "Spirit form ready!";
		butterflyUI.transform.Find("ButterflyCount").GetComponent<Text>().text = text;
	}
}
