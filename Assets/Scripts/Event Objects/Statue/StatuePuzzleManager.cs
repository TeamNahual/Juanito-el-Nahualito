using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class StatuePuzzleManager : MonoBehaviour {

	public RelicPedestal relic;

	private static StatuePuzzleManager statuePuzzleManagerinstance;

	public List<StatueContainer> statues;

	public static StatuePuzzleManager ins
	{
		get 
		{
			if (statuePuzzleManagerinstance == null) 
			{
				statuePuzzleManagerinstance = new GameObject ("StatuePuzzleManager").AddComponent<StatuePuzzleManager> ();
			}
			return statuePuzzleManagerinstance;
		}
				
	}

	void Awake()
	{
		statuePuzzleManagerinstance = this;

	}

	void FixedUpdate()
	{
		HandleSpirit();
	}

	void HandleSpirit()
	{
		transform.GetChild(0).gameObject.SetActive(Juanito.ins.SpiritState);

	}

	bool NextStatue()
	{
		statues.RemoveAt(0);

		if(statues.Count == 0)
			return false;

		GameObject statue = statues[0].gameObject;

		float yIncrement = statue.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.y;

		statue.SetActive(true);
		// statue.transform.parent.Translate(0, Mathf.Abs(yIncrement), 0);
		StartCoroutine(RaiseStatue(statue.transform, yIncrement));

		return true;
	}

	IEnumerator RaiseStatue(Transform statueTrans, float increment)
	{
		float k = 0;

		Vector3 beginningPosition = statueTrans.parent.position;
		Vector3 finalPosition = beginningPosition + new Vector3(0, increment, 0); 
		while (k < 1) 
		{
			statueTrans.parent.position = Vector3.Lerp (beginningPosition, finalPosition, k);

			k += Time.deltaTime;
			yield return null;
		}
	}

	public void CheckStatueRotations()
	{
		if(!NextStatue())
		{
			StartCoroutine(relic.RaisePedestal());
		}
	}
}
