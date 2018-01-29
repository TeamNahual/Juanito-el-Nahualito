using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juanito : MonoBehaviour {

	public static Juanito ins;

	public GameObject JuanitoHuman;

	private int numberOfButterflies = 0;

	int MAX_SPIRIT_COUNT = 3;

	// Use this for initialization
	void Awake () {
		ins = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int GetSpiritCount()
	{
		return numberOfButterflies;
	}

	public bool AddSpiritCount(int count)
	{
		if(numberOfButterflies < MAX_SPIRIT_COUNT)
		{
			numberOfButterflies += count;
			return true;
		}
		
		return false;
	}

	public void DelSpiritCount(int count)
	{
		numberOfButterflies = Mathf.Max(numberOfButterflies - count, 0); 
	}
}
