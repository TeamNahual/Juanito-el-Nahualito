using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance; // Allows us to access this from other scripts
	public bool isMovementLocked = false; // Should this be somewhere else?
	public int movementLocks = 0;
	private int unlockCounter;

	public string levelToLoad;

	public static GameManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GameObject("GameManager").AddComponent<GameManager>();
			}
			return _instance;
		}
	}

	void Awake()
	{
		if (_instance == null)
			_instance = this;
		else if (_instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}
	
    void Start()
    {
		DynamicGI.UpdateEnvironment();
	}
	
	void Update() {
		if (movementLocks == 0 && isMovementLocked) {
			if (--unlockCounter == 0) {
				isMovementLocked = false;
			}
		}
	}
	
	public void lockMovement() {
		isMovementLocked = true;
		movementLocks++;
	}
	
	public void unlockMovement() {
		--movementLocks;
		if (movementLocks == 0) {
			unlockCounter = 3;
		}
	}

	void resetMovement()
	{
		movementLocks = 0;
		unlockCounter = 0;
		isMovementLocked = false;
	}
	
	public void reloadScene()
	{
		resetMovement();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		DynamicGI.UpdateEnvironment();
	}

	public void loadLevel(string sceneName)
	{
		resetMovement();
		levelToLoad = sceneName;
		SceneManager.LoadScene (sceneName);
	}
	
	public void exitGame()
	{
		Application.Quit();
	}
}