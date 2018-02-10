using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueEvent : EventObject {

	public bool pushing = false;
	public int rotateFlag = 1;

	public StatueContainer container; 

	int PUSH_DEGREES = 30;
	int PUSH_DURATION = 3;

	void OnTriggerStay(Collider other) {

		if(Input.GetKeyDown(KeyCode.E) && !pushing)
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

		while(k < duration)
		{
			Juanito.ins.SpiritControl.currentFollower.transform.parent = transform.parent;
			transform.parent.transform.Rotate(Vector3.up * Time.deltaTime * degrees/duration);
			k += Time.deltaTime;
			yield return null;
		}

		container.currentRotation += rotateFlag * PUSH_DEGREES;

		StatuePuzzleManager.ins.CheckStatueRotations ();

		Juanito.ins.SpiritControl.currentFollower.transform.parent = null;
		pushing = false;
	}

	public void TriggerEvent()
	{
		StartCoroutine(RotateStatue(rotateFlag * PUSH_DEGREES, PUSH_DURATION));
	}
}
