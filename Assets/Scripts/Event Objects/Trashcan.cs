using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class Trashcan : EventObject {
    //use the setDialogue function to change this value.
    public string firstDia = "You talked to the guy."; // first thing npc says

    void OnTriggerStay(Collider other)//if an object is in the box around the old man
    {
        if (other.gameObject != Juanito.ins.JuanitoHuman) //if it is not Juanito do nothing.
        {
            return;
        }

        Vector3 fwd = Juanito.ins.JuanitoHuman.transform.TransformDirection(Vector3.forward);
        Vector3 dir = Vector3.Normalize(transform.position - Juanito.ins.JuanitoHuman.transform.position);
        bool dirCheck = Vector3.Dot(fwd, dir) > 0.5;
        
        if (dirCheck)//if player is facing the person, turn on the tool tip.
        {
            UIManager.instance.TooltipDisplay("Press <sprite=0> to get food"); //displays message saying press X to talk
        }
        if (!dirCheck) //if player is not facing the person, turn off the tool tip.
        {
            UIManager.instance.TooltipDisable(); //turns off the tooltips
            UIManager.instance.toggleDialogueBox(false);
        }
        if ((Input.GetKeyDown(KeyCode.E) || CrossPlatformInputManager.GetButtonDown("Action")) && dirCheck) //checks if interact button was pressed
        {
            UIManager.instance.dialogueSystem.doDialogueAction();
            UIManager.instance.dialogueSystem.addDialogue(firstDia);
            Juanito.ins.hasFood = true;
            return;
        }
    }
    //Periodically a couple of bugs if the player is too close to the edge of the collider
    void OnTriggerExit(Collider other) //when an object moves out of box
    {
        if (other.gameObject == Juanito.ins.JuanitoHuman)//if object is Juanito clear the tool tip message
        {
            UIManager.instance.TooltipDisable(); //turns off the tooltips
            UIManager.instance.toggleDialogueBox(false); //turns off the dialogue boxes
            //closes the dialogue box
        }
    }
}
