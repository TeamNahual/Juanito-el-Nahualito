using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class StatuePuzzleManager : MonoBehaviour {

	public RelicPedestal relic;

	private static StatuePuzzleManager statuePuzzleManagerinstance;

	public List<StatueContainer> statues;

	public bool completed = false;
	public GameObject pyramidDoor;
	private bool doorOpened = false;

	//Completion Audio Install
	private AudioSource myAudioSource;
	public AudioClip[] RotationPuzzleSounds = new AudioClip[0];

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
		myAudioSource = GetComponent<AudioSource> ();
		statuePuzzleManagerinstance = this;

	}

	void FixedUpdate()
	{
		HandleSpirit();
		ManageDoor();
	}

	void HandleSpirit()
	{
		transform.GetChild(0).gameObject.SetActive(Juanito.ins.SpiritState);

	}

	void ManageDoor()
	{
		if(!completed)
			return; 

		if(!relic.active && !doorOpened)
			StartCoroutine(OpenDoor());
	}

	IEnumerator OpenDoor()
	{
		doorOpened = true;
		myAudioSource.PlayOneShot (RotationPuzzleSounds [1], 1);

		Transform doorLeft = pyramidDoor.transform.GetChild(0);
		Vector3 LStart = doorLeft.localPosition;
		Vector3 LEnd = new Vector3(LStart.x, LStart.y, 4.86f);

		Transform doorRight = pyramidDoor.transform.GetChild(1);
		Vector3 RStart = doorRight.localPosition;
		Vector3 REnd = new Vector3(RStart.x, RStart.y, -2.08f);

		float k = 0;

		while (k < 2) 
		{
			doorLeft.localPosition = Vector3.Lerp(LStart, LEnd, k);
			doorRight.localPosition = Vector3.Lerp(RStart, REnd, k);

			k += Time.deltaTime * Random.Range(0.5f,1f);
			yield return null;
		}

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
			completed = true;
			myAudioSource.PlayOneShot (RotationPuzzleSounds [0], 1);
		}
	}

}
