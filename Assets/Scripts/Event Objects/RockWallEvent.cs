using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockWallEvent : EventObject {

	public bool pushing = false;

	int DIG_DISTANCE = 10;
	int DIG_DURATION = 3;

	void OnTriggerStay(Collider other) 
	{
		if(Input.GetKeyDown(KeyCode.E) && !pushing)
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
			transform.parent.transform.Translate(Vector3.up * Time.deltaTime * DIG_DISTANCE/DIG_DURATION);
			k += Time.deltaTime;
			yield return null;
		}
	}

	public void TriggerEvent()
	{
		StartCoroutine(Event());
	}
}
