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
	
	// Dialogue System
	public GameObject dialogueUI;
	public DialogueSystem dialogueSystem;
	private bool dialogueToggleProtect;
    private bool isDialogueOpen;
	public AudioSource dialogueAudioSource;
	
	// Butterflies
	public GameObject butterflyUI;

	//Audio Mixer Snapshots
	public AudioMixerSnapshot paused;
	public AudioMixerSnapshot mainGame;
	public AudioMixerSnapshot voiceOver;

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

				// Will Call Funcition to Lower Audio Volume when Paused
				AudioPause ();
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
		
		// Update butteflies
		updateButterflyUI(Juanito.ins.GetSpiritCount());
		
		inGameMenu.gameObject.SetActive(isMenuOpen);
		dialogueUI.gameObject.SetActive(isDialogueOpen);
	}
	
	public void closeMainMenu()
	{
		isMenuOpen = false;
	}
	
	public void toggleDialogueBox(bool toggle) {
		isDialogueOpen = toggle;

		// Control Audio When Juanito's Grandpa is Talking
		if (toggle) {
			voiceOver.TransitionTo (.01f);
		}

		if (!toggle) {
			mainGame.TransitionTo (.01f);
		}
	}
	
	public void setDialogueBoxText(string text) {
		dialogueUI.transform.Find("Dialogue").GetComponent<Text>().text = text;
	}
	
	public void setAndPlayAudioClip(AudioClip clip) {
		dialogueAudioSource.clip = clip;
		dialogueAudioSource.Play();
	}
	
	private void updateButterflyUI(int count) {
		string text = (count < 3)? "Butterflies: " + count + " / 3": "Spirit form ready!";
		butterflyUI.transform.Find("ButterflyCount").GetComponent<Text>().text = text;
	}

	// Used For Bringing down the volume of things when Pausing or entering the menu
	public void AudioPause(){
		if (isMenuOpen) {
			paused.TransitionTo (.01f);
		}

		if (!isMenuOpen) {
			mainGame.TransitionTo (.01f);
		}
	}
}
