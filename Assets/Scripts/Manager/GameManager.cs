using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null; // Allows us to access this from other scripts
	private bool dialogueTest;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
		dialogueTest = false;
	}
	
    void Start()
    {
		DynamicGI.UpdateEnvironment();
	}
	
	void Update()
	{
		if (Input.GetKey(KeyCode.Z) && !dialogueTest) {
			UIManager.instance.addDialogueString("Test - Solo");
			string[] arr = {"Test1", "Test2"};
			UIManager.instance.addDialogueString(arr);
		}
		dialogueTest = Input.GetKey(KeyCode.Z);
	}
	
	public void reloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		DynamicGI.UpdateEnvironment();
	}
	
	public void exitGame()
	{
		Application.Quit();
	}
}