using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyPuzzle : MonoBehaviour {

	public ButterflyPiece[] pieces;
	public MeshRenderer butterflyOutline;
	public bool allActive = false;
	public bool debug = false;
	public RelicPedestal relic;

	public static ButterflyPuzzle ins;

	private AudioSource aura;

	// Use this for initialization
	void Awake () {
		ins = this;

		butterflyOutline.gameObject.SetActive (false);

		butterflyOutline.materials[0].SetColor ("_ContourColor", new Color (1, 1, 1, 0));
		butterflyOutline.materials[1].SetColor ("_ContourColor", new Color (1, 1, 1, 0));

		aura = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (debug) {
			StartCoroutine (GlowOutline ());
			debug = false;
		}
	}

	public bool CheckPieces()
	{
		for (int i = 0; i < pieces.Length; i++) 
		{
			if (!pieces [i].locked) 
				return false;
		}

		StartCoroutine (GlowOutline ());
		allActive = true;
		UIManager.instance.dialogueSystem.addDialogue("You have completed the butterfly puzzle.");
		return true;
	}

	IEnumerator GlowOutline()
	{
		butterflyOutline.gameObject.SetActive (true);
		aura.volume = 0;
		aura.Play ();

		float a = 0;

		while (a < 0.8f) 
		{
			aura.volume = a/2;
			butterflyOutline.materials[0].SetColor ("_ContourColor", new Color (1, 1, 1, a));
			butterflyOutline.materials[1].SetColor ("_ContourColor", new Color (1, 1, 1, a));

			a += 0.005f;
			yield return null;
		}

		while (a > 0) 
		{
			aura.volume = a/2;
			butterflyOutline.materials[0].SetColor ("_ContourColor", new Color (1, 1, 1, a));
			butterflyOutline.materials[1].SetColor ("_ContourColor", new Color (1, 1, 1, a));

			a -= 0.005f;
			yield return null;
		}

		aura.Stop ();

		butterflyOutline.gameObject.SetActive (false);


		StartCoroutine (relic.RaisePedestal ());
	}
}
