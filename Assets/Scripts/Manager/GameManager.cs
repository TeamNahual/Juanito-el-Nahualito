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
	
	// Shader Related
	public Texture emptyGradient, stylisticFog, desaturationGradient, spiritModeGradient;
	public Vector3 spiritModeOrigin;
	public float spiritModeRadius;

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
		Shader.SetGlobalTexture("_MK_FOG_DESATURATE", desaturationGradient);
		Shader.SetGlobalTexture("_MK_FOG_STYLISTIC", stylisticFog);
		Shader.SetGlobalTexture("_MK_FOG_SPIRIT_MODE_GRADIENT", spiritModeGradient);
		Shader.SetGlobalVector("_MK_FOG_SPIRIT_MODE_ORIGIN", spiritModeOrigin);
		Shader.SetGlobalFloat("_MK_FOG_SPIRIT_MODE_RADIUS", spiritModeRadius);
		Shader.SetGlobalInt("_MK_FOG_SPIRIT_MODE_ENABLED", 0);
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
		SceneManager.LoadScene ("LoadingScreen");
	}
	
	public void exitGame()
	{
		Application.Quit();
	}
}