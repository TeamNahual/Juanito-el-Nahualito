using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatuePuzzleManager : MonoBehaviour {

	private static StatuePuzzleManager statuePuzzleManagerinstance;

	public List<StatueContainer> statues;

	public static StatuePuzzleManager ins
	{
		get 
		{
			if (statuePuzzleManagerinstance == null) 
			{
				statuePuzzleManagerinstance = new GameObject ("StatuePuzzleManager").AddComponent<StatuePuzzleManager> ();
			}
			return statuePuzzleManagerinstance;
		}
				
	}

	void Awake()
	{
		statuePuzzleManagerinstance = this;
		statues = new List<StatueContainer> ();
	}

	public bool CheckStatueRotations()
	{
		foreach (StatueContainer statue in statues)
		{
			if (statue.currentRotation != statue.requiredRotation)
			{
				return false;
			}
		}
	
		return true;
	}
}
