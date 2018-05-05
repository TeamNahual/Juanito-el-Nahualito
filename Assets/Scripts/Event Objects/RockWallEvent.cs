using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockWallEvent : EventObject {

	public bool pushing = false;
	public bool disabled = false;

	int DIG_DISTANCE = 10;
	int DIG_DURATION = 3;

	void OnTriggerEnter(Collider other) 
	{
		if(!pushing && !disabled)
		{//if juanito is in spirit mode and has a follower which is a badger then run task
			if(other.gameObject == Juanito.ins.JuanitoSpirit && Juanito.ins.SpiritControl.currentFollower)
			{
				Tejon tejonController = Juanito.ins.SpiritControl.currentFollower.GetComponent<Tejon> ();
				if(tejonController)
				{
					pushing = true;
					tejonController.RunTask(transform);
				}
			}
		}
	}

	IEnumerator Event() //just makes it go into the ground, more unique based on what you want it to do
	{
		float k = 0;

		while(k < DIG_DURATION)
		{
			transform.parent.transform.Translate(Vector3.down * Time.deltaTime * DIG_DISTANCE/DIG_DURATION);
			k += Time.deltaTime;
			yield return null;
		}

		disabled = true;
	}

	public void TriggerEvent()
	{
		StartCoroutine(Event()); //starts the above routine
	}
}
