using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueEvent : EventObject {

	bool pushing = false;
	int PUSH_DEGREES = 30;
	int PUSH_DURATION = 3;

	void OnTriggerStay(Collider other) {

		if(Input.GetKeyDown(KeyCode.E) && !pushing)
		{
			if(other.gameObject == Juanito.ins.JuanitoSpirit && Juanito.ins.SpiritControl.currentFollower)
			{
				if(CheckPlayerDirection(other.gameObject) && Juanito.ins.SpiritControl.currentFollower.GetComponent<AIFollowController>().animal == ReqAnimal)
				{
					Vector3 playerRelative = other.gameObject.transform.InverseTransformPoint(transform.position);
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
	}

	IEnumerator RotateStatue(float degrees, float duration)
	{
		pushing = true;
		float k = 0;

		while(k < duration)
		{
			Juanito.ins.transform.parent = transform.parent;
			transform.parent.transform.Rotate(Vector3.up * Time.deltaTime * degrees/duration);
			k += Time.deltaTime;
			yield return null;
		}

		Juanito.ins.transform.parent = null;
		pushing = false;
	}
}
