using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGramps : MonoBehaviour {

    public Animator myAnimator;
    public AudioSource monologue;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (!monologue.isPlaying && monologue.isActiveAndEnabled)
        {
            myAnimator.SetTrigger("Died");
        }
	}
}
