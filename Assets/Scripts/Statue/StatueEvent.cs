using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueEvent : EventObject {

	public bool pushing = false;
	int PUSH_DEGREES = 30;
	int PUSH_DURATION = 3;

	void OnTriggerStay(Collider other) {

		if(Input.GetKeyDown(KeyCode.E) && !pushing)
		{
			if(other.gameObject == Juanito.ins.JuanitoSpirit && Juanito.ins.SpiritControl.currentFollower)
			{
				Juanito.ins.SpiritControl.currentFollower.GetComponent<Deer>().RunTask(transform);
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

		Juanito.ins.SpiritControl.currentFollower.transform.parent = null;
		pushing = false;
	}

	public void TriggerEvent()
	{
		if(CheckPlayerDirection(Juanito.ins.gameObject) && Juanito.ins.SpiritControl.currentFollower.GetComponent<AIFollowController>().animal == ReqAnimal)
		{
			Vector3 playerRelative = Juanito.ins.gameObject.transform.InverseTransformPoint(transform.position);
			if(playerRelative.x > 0)
			{
				StartCoroutine(RotateStatue(-PUSH_DEGREES, PUSH_DURATION));
			}
			else
			{
				StartCoroutine(RotateStatue(PUSH_DEGREES, PUSH_DURATION));
			}
		}
	}
}
