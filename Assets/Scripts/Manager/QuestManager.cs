using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class Quest
{
	public string questTitle;
	public string questSlug;
	public bool active;
	public bool completed;
	public string nextQuest;

	public Quest(string title, string slug, string next = "")
	{

	}

	public ActivateQuest()
	{

	}

	public CompleteQuest()
	{
		
	}
}
