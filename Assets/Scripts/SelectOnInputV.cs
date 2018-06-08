using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using DoozyUI;

public class SelectOnInputV : MonoBehaviour {

	public EventSystem eventSystem;
	public GameObject selectedObject;

	public GameObject[] selectables;

	public int counter = 0;

	private bool buttonSelected;
	private bool controllerDown = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		float v = Input.GetAxisRaw ("Vertical-Joystick");

		Debug.Log(v);

		if (!controllerDown && v != 0)
		{
			eventSystem.SetSelectedGameObject(selectedObject);
			if(v > 0)
				counter--;
			else if(v < 0)
				counter++;

			if(counter >= selectables.Length)
				counter = 0;
			
			if(counter < 0)
				counter = selectables.Length - 1;

			selectedObject = selectables[counter];

			controllerDown = true;
			StartCoroutine(SelectGameObjectLater(selectedObject));
			// buttonSelected = true;
			selectedObject.GetComponent<UIButton>().ExecuteOnPointerEnter(true);
		}
		else
		{
			if(v == 0)
				controllerDown = false;
		}
	}
	IEnumerator SelectGameObjectLater(GameObject objectToSelect)
	{
		yield return null;
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(objectToSelect);
	}
	private void OnDisable()
	{
		buttonSelected = false;
	}
}
