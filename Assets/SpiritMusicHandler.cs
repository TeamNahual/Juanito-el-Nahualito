using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Audio;

public class SpiritMusicHandler : MonoBehaviour {

	//public GameObject JuanitoObject;
	public bool checkSpiritChange;

	public AudioMixerSnapshot spiritMusic;
	public AudioMixerSnapshot mainMusic;

	//public AudioMixer mainMusic;
	//public AudioMixer spiritMusic;

	// Use this for initialization
	//void Start () {
		//spiritMusic = gameObject.GetComponent<AudioSource> ().clip;
	//}
	
	// Update is called once per frame
	void Update () {
		checkSpiritChange = Juanito.ins.SpiritState;
		//Debug.Log (checkSpiritChange);
		if (Input.GetKeyDown (KeyCode.Q) || CrossPlatformInputManager.GetButtonDown("Toggle-Spirit")) {
			if (!checkSpiritChange && Juanito.ins.GetSpiritCount() >= 1) {
				//Debug.Log (checkSpiritChange);
				spiritMusic.TransitionTo(.25f);
				//spiritMusic.Play ();
			}
			if (checkSpiritChange) {
				mainMusic.TransitionTo (.25f);
			}
		}
		if (Juanito.ins.GetSpiritCount() <= 0) {
			mainMusic.TransitionTo (.25f);
		}
	}
}
