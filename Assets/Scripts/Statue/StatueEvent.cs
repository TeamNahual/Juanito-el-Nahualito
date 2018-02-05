using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueEvent : EventObject {

	bool pushing = false;

	void OnTriggerStay(Collider other) {

		if(Input.GetKeyDown(KeyCode.E) && !pushing && CheckPlayerDirection(other.gameObject))
		{
			Debug.Log("Object Interacted With");
			StartCoroutine(RotateStatue());
		}
	}

	IEnumerator RotateStatue()
	{
		pushing = true;
		float k = 0;

		while(k < 3)
		{
			Juanito.ins.transform.parent = transform.parent;
			transform.parent.transform.Rotate(Vector3.up * Time.deltaTime * 45/3);
			k += Time.deltaTime;
			yield return null;
		}

		Juanito.ins.transform.parent = null;
		pushing = false;
	}
}
