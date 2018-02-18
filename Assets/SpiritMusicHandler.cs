using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Audio;

public class SpiritMusicHandler : MonoBehaviour {

	public GameObject JuanitoObject;
	public bool checkSpiritChange;
	public AudioSource spiritMusic;
	public AudioSource mainMusic;

	// Use this for initialization
	//void Start () {
		//spiritMusic = gameObject.GetComponent<AudioSource> ().clip;
	//}
	
	// Update is called once per frame
	void Update () {
		checkSpiritChange = JuanitoObject.GetComponent<Juanito> ().SpiritState;
		//Debug.Log (checkSpiritChange);
		if (Input.GetKeyDown (KeyCode.Q) || CrossPlatformInputManager.GetButtonDown("Toggle-Spirit")) {
			if (!checkSpiritChange && Juanito.ins.GetSpiritCount() >= 3) {
				//Debug.Log (checkSpiritChange);
				mainMusic.Stop();
				spiritMusic.Play ();
			}
			if (checkSpiritChange) {
				spiritMusic.Stop ();
				mainMusic.Play ();
			}
		}
	}
}
