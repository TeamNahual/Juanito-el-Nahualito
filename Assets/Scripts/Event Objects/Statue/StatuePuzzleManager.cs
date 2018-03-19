using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class StatuePuzzleManager : MonoBehaviour {

	public RelicPedestal relic;

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
			if (!statue.disabled)
			{
				return false;
			}
		}
		
		if(ButterflyPuzzle.ins.allActive)
		{
			// UIManager.instance.dialogueSystem.addDialogue("You have completed both puzzles of this build. There will be more puzzles for Juanito in his search for his grandpa. To be continued...");
		}
		else
		{
			//UIManager.instance.dialogueSystem.addDialogue("You have completed the statue puzzle.");
		}

		StartCoroutine(relic.RaisePedestal());
		return true;
	}
}
