using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager instance = null; // Allows us to access this from other scripts
	
	// Menu Related
	public GameObject inGameMenu;
	private bool menuToggleProtect;
    private bool isMenuOpen;
	
	// Dialogue System
	private Queue dialogueQueue;
	public GameObject dialogueUI;
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
		
		dialogueQueue = new Queue();
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
		
		if (Input.GetKey(KeyCode.Return)) {
			if (!dialogueToggleProtect) {
				doDialogueAction();
			}
			dialogueToggleProtect = true;
		} else if (dialogueToggleProtect) {
			dialogueToggleProtect = false;
		}
		
		inGameMenu.gameObject.SetActive(isMenuOpen);
		dialogueUI.gameObject.SetActive(isDialogueOpen);
	}
	
	public void closeMainMenu()
	{
		isMenuOpen = false;
	}
	
	// Dialogue Actions
	private void updateDialogueText()
	{
		isDialogueOpen = dialogueQueue.Count > 0;
		if (isDialogueOpen) {
			dialogueUI.transform.Find("Dialogue").GetComponent<Text>().text = (string) dialogueQueue.Peek();
		}
	}
	
	private void doDialogueAction()
	{
		if (dialogueQueue.Count > 0) {
			dialogueQueue.Dequeue();
		}
		updateDialogueText();
	}
	
	public void addDialogueString(string text)
	{
		dialogueQueue.Enqueue(text);
		updateDialogueText();
	}
	
	public void addDialogueString(string[] text)
	{
		for (int i = 0; i < text.Length; ++i) {
			dialogueQueue.Enqueue(text[i]);
		}
		updateDialogueText();
	}
}
