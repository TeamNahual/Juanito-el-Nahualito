using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRollEvent : EventObject {

	public bool pushing = false;

	public int PUSH_DISTANCE = -90;
	int PUSH_DURATION = 3;

	void OnTriggerEnter(Collider other) 
	{
		if(!pushing)
		{
			if(other.gameObject == Juanito.ins.JuanitoSpirit && Juanito.ins.SpiritControl.currentFollower)
			{
				Deer deerController = Juanito.ins.SpiritControl.currentFollower.GetComponent<Deer> ();
				if(deerController)
				{
					pushing = true;
					deerController.RunTask(transform);
				}
			}
		}
	}

	IEnumerator Event()
	{
		float k = 0;

		while(k < PUSH_DURATION)
		{
			transform.parent.transform.Translate(Vector3.back * Time.deltaTime * PUSH_DISTANCE/PUSH_DURATION);
			// transform.parent.transform.Rotate(Vector3.back);
			k += Time.deltaTime;
			yield return null;
		}
	}

	public void TriggerEvent()
	{
		StartCoroutine(Event());
	}
}
