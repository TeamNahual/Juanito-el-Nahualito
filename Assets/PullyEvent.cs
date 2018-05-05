using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Playables;

public class PullyEvent : EventObject {

    public bool moving = false;
    public bool disabled = false;
	public PlayableDirector timelineDirector;

    void OnTriggerStay(Collider other)
    {
        if (!moving && !disabled)
        {//if juanito is in spirit mode and has a follower which is a badger then run task            
            if (other.gameObject == Juanito.ins.JuanitoHuman && Juanito.ins.SpiritControl.currentFollower)
            {
                if ((Input.GetKeyDown(KeyCode.E)))
                {
                    Dog dogController = Juanito.ins.SpiritControl.currentFollower.GetComponent<Dog>();
                    if (dogController)
                    {
                        moving = true;
                        dogController.RunTask(transform);
                    }
                }
            }

        }
    }

    public void TriggerEvent()
    {
		timelineDirector.Play ();
        Debug.Log("Dog jumps in basket");
        //disabled = true;
    }
}
