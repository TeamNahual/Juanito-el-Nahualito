using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance; // Allows us to access this from other scripts
	public bool isMovementLocked = false; // Should this be somewhere else?

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
	
	void Update()
	{
	}	
	
	public void reloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		DynamicGI.UpdateEnvironment();
	}

	public void loadLevel(string sceneName)
	{
		levelToLoad = sceneName;
		SceneManager.LoadScene ("LoadingScreen");
	}
	
	public void exitGame()
	{
		Application.Quit();
	}
}