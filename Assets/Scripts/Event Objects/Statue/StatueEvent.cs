using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueEvent : EventObject {

	public bool pushing = false;
	public int rotateFlag = 1;
	public bool disable = false;

	public StatueContainer container; 

	int PUSH_DEGREES = 30;
	int PUSH_DURATION = 3;

	void OnTriggerEnter(Collider other) {

		if(!pushing && !disable)
		{
			if(other.gameObject == Juanito.ins.JuanitoSpirit && Juanito.ins.SpiritControl.currentFollower)
			{
				Deer deerController = Juanito.ins.SpiritControl.currentFollower.GetComponent<Deer> ();
				if(deerController)
					deerController.RunTask(transform);
			}
		}
	}

	IEnumerator RotateStatue(float degrees, float duration)
	{
		pushing = true;
		float k = 0;

		Juanito.ins.SpiritControl.currentFollower.transform.parent = transform.parent;
		
		while(k < duration)
		{
			transform.parent.transform.Rotate(Vector3.up * Time.deltaTime * degrees/duration);
			k += Time.deltaTime;
			yield return null;
		}

		container.currentRotation += rotateFlag * PUSH_DEGREES;

		if(container.currentRotation == container.requiredRotation)
			disable = true;

		StatuePuzzleManager.ins.CheckStatueRotations ();

		Juanito.ins.SpiritControl.currentFollower.transform.parent = null;
		pushing = false;
	}

	public void TriggerEvent()
	{
		StartCoroutine(RotateStatue(rotateFlag * PUSH_DEGREES, PUSH_DURATION));
	}
}
