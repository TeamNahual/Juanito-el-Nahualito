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
		{
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

	IEnumerator Event()
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
		StartCoroutine(Event());
	}
}
