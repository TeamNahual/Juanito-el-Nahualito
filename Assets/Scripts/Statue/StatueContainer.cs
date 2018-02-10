using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueContainer : MonoBehaviour {

	public int currentRotation = 0;
	public int requiredRotation = 30;

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
	}
}
