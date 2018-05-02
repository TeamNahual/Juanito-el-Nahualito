using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VoiceTemple2 : MonoBehaviour {
		public AudioMixerSnapshot templesVo;
		//public AudioMixerSnapshot previous;


		void OnTriggerEnter(Collider other) {

			if(other.gameObject != Juanito.ins.JuanitoHuman)
				return;

			templesVo.TransitionTo (.25f);
		}
	}
