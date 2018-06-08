using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Audio;

public class Spirit_Transformation : MonoBehaviour {

	public AudioSource mySource;
	public AudioClip[] myClip = new AudioClip[0];
	public AudioMixerSnapshot[] snapshot = new AudioMixerSnapshot[0];

	// Use this for initialization
	void Start () {
		mySource = gameObject.GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		if(Juanito.ins.inButterflyZone)
		{
			if(Input.GetKeyDown(KeyCode.Q) || CrossPlatformInputManager.GetButtonDown("Toggle-Spirit"))
			{
				if(!Juanito.ins.SpiritState)
				{
					// lockedSpiritPosition = JuanitoHuman.transform.position;
					mySource.Play ();
					mySource.PlayOneShot(myClip[0], 1);
					mySource.PlayOneShot (myClip [1], 0.75f);
					//mySource.PlayOneShot (myClip [2], 1);
					mySource.PlayOneShot (myClip [3], 1);
					//snapshot [1].TransitionTo (0.25f);
				}
	
				else
				{
					mySource.Stop ();
					snapshot [0].TransitionTo (1f);
				}
			}
			/*if(SpiritState)
			{
				// JuanitoHuman.transform.position = lockedSpiritPosition;
			}*/
		}

		if (Juanito.ins.SpiritState) {
			snapshot [1].TransitionTo (0.25f);
			//mySource.Stop ();
		}
	}
}
