using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null; // Allows us to access this from other scripts

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
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
}