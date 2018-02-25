using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueContainer : MonoBehaviour {

	public float currentRotation;
	public float requiredRotation = 30;
	public float bounds = 1;
	public bool disabled = false;

	void Start()
	{
		StatuePuzzleManager.ins.statues.Add (this);
	}

	void Update()
	{
		if (currentRotation < 0)
			currentRotation += 360;
		else if (currentRotation > 360)
			currentRotation -= 360;

		if(currentRotation >= requiredRotation - 1 && currentRotation <= requiredRotation + 1)
		{
			disabled = true;
		}
	}
}
