using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoozyUI;

public class AnyButtonPress : MonoBehaviour {

	public GameObject TitleButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.anyKeyDown)
		{
			TitleButton.GetComponent<UIButton>().ExecuteClick();
		}
	}
}
