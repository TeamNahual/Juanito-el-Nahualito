using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalTransformation : MonoBehaviour {

	public GameObject characterModel;
	public GameObject quetzalModel;

	// Use this for initialization
	void Start () {
		
	}


	// Update is called once per frame
	void Update () {
		TransformToAnimal();
	}

	private void TransformToAnimal()
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			characterModel.SetActive(!characterModel.activeSelf);
			quetzalModel.SetActive(!quetzalModel.activeSelf);
		}
	}
}
