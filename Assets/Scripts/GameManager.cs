using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool isMenuOpen;
	private bool menuToggleProtect;

    void Start()
    {
		DynamicGI.UpdateEnvironment();
        isMenuOpen = false;
		menuToggleProtect = false;
    }
	
	void Update()
	{
		if (Input.GetKey(KeyCode.Escape)) {
			if (!menuToggleProtect) {
				isMenuOpen = !isMenuOpen;
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
			menuToggleProtect = true;
		} else if (menuToggleProtect) {
			menuToggleProtect = false;
		}
	}
}