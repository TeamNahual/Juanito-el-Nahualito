using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyPuzzle : MonoBehaviour {

	public ButterflyPiece[] pieces;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool CheckPieces()
	{
		for (int i = 0; i < pieces.Length; i++) 
		{
			if (!pieces [i].locked)
				return false;
		}

		return true;
	}
}
