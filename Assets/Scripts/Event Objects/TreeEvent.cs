using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEvent : EventObject {

	public bool pushing = false;

	int PUSH_DEGREES = -86;
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

	IEnumerator RotateTree()
	{
		float k = 0;
		float rate = 1;

		while(k < PUSH_DURATION)
		{
			transform.parent.transform.Rotate(Vector3.right * Time.deltaTime * rate * PUSH_DEGREES/PUSH_DURATION);
			k += Time.deltaTime * rate;

			rate += 0.02f;
			yield return null;
		}
	}

	public void TriggerEvent()
	{
		StartCoroutine(RotateTree());
	}
}
