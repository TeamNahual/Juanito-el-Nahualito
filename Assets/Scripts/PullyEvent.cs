using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Playables;

public class PullyEvent : EventObject {

    public bool moving = false;
    public bool disabled = false;
	//public PlayableDirector timelineDirector;
    public GameObject connection; 

    void OnTriggerStay(Collider other)
    {


        if (!moving && !disabled)
        {//if juanito is in spirit mode and has a follower which is a badger then run task     

            Vector3 fwd = Juanito.ins.JuanitoHuman.transform.TransformDirection(Vector3.forward);
            Vector3 dir = Vector3.Normalize(transform.position - Juanito.ins.JuanitoHuman.transform.position);
            bool dirCheck = Vector3.Dot(fwd, dir) > 0.5;

            if (dirCheck)//if player is facing the person, turn on the tool tip.
            {
                UIManager.instance.TooltipDisplay("Press <sprite=0> to place food"); //displays message saying press X to talk
            }
            if (!dirCheck) //if player is not facing the person, turn off the tool tip.
            {
                UIManager.instance.TooltipDisable(); //turns off the tooltips
                UIManager.instance.toggleDialogueBox(false);
            }

            if (other.gameObject == Juanito.ins.JuanitoHuman && Juanito.ins.SpiritControl.currentFollower)
            {
                if ((Input.GetKeyDown(KeyCode.E) || CrossPlatformInputManager.GetButtonDown("Action"))) //edit to accept controller input
                {
                    Dog dogController = Juanito.ins.SpiritControl.currentFollower.GetComponent<Dog>();
                    if (dogController)
                    {
                        Juanito.ins.hasFood = false;
                        moving = true;
                        dogController.RunTask(transform);
                    }
                }
            }

        }
    }

    void OnTriggerExit(Collider other) //when an object moves out of box
    {
        if (other.gameObject == Juanito.ins.JuanitoHuman)//if object is Juanito clear the tool tip message
        {
            UIManager.instance.TooltipDisable(); //turns off the tooltips
        }
    }

    public void TriggerEvent()
    {
        //Test this to make sure dog is getting on platform and Juanito is out of way.
        Juanito.ins.SpiritControl.currentFollower.transform.position = connection.transform.position;
        Juanito.ins.hasFood = false;
        if(connection.GetComponent<pulley_platform>())
        {
            connection.GetComponent<pulley_platform>().Activate(Juanito.ins.SpiritControl.currentFollower.transform);
        }
        else
        {
            Debug.Log("You broke it");
        }
        
        Debug.Log("Play transform");
        //timelineDirector.Play ();
        //disabled = true;
    }
}
