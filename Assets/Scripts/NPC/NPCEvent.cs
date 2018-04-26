using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class NPCEvent : EventObject {
    //use the setDialogue function to change this value.
    private int dialogueState = 0; //0 is never talked to, 1 is talked to once, 2 is talked to and done quest

    //dialogue for NPC, public so we can tweak it without going into code.
    public string firstDia = "You talked to the guy."; // first thing npc says
    public string secondDia = "You talked to the guy again."; //thing npc says after talked to once
    public string thirdDia = "You talked to the guy thrice."; //thing npc says after completed task

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
            UIManager.instance.TooltipDisplay("Hold <sprite=0> to talk"); //displays message saying press X to talk
        }
        if (!dirCheck) //if player is not facing the person, turn off the tool tip.
        {
            UIManager.instance.TooltipDisable(); //turns off the tooltips
            UIManager.instance.toggleDialogueBox(false);
        }
        

        if ((Input.GetKeyDown(KeyCode.E) || CrossPlatformInputManager.GetButtonDown("Action")) && dirCheck) //checks if interact button was pressed
        {
            switch (dialogueState)//handles the state of the dialogue
            {
                case 0: // state when player hasn't talked to NPC
                    UIManager.instance.dialogueSystem.doDialogueAction();
                    UIManager.instance.dialogueSystem.addDialogue(firstDia); //calls the UI manager to print the dialogue
                    dialogueState = 1; //player has talked to NPC
                    break;
                case 1: //state when player has talked to NPC once, haven't completed quest yet
                    UIManager.instance.dialogueSystem.doDialogueAction(); //clears previous dialogue
                    UIManager.instance.dialogueSystem.addDialogue(secondDia); //calls the UI manager to print the dialogue
                    break;
                case 2: //player has completed quest
                    UIManager.instance.dialogueSystem.doDialogueAction(); //clears previous dialogue
                    UIManager.instance.dialogueSystem.addDialogue(thirdDia); //calls the UI manager to print the dialogue
                    break;
                default:
                    Debug.Log("You broke my code.");
                    break;
            }
        }
        /**
        //for testing purposes only, remove from final code.
        if (Input.GetKeyDown(KeyCode.R)) // used for testing to check if state 2 works.
        {
            if (dialogueState != 2)// sets dialogue to 2 to test if possible to switch the dialogue state.
            {
                setDialogue(2);
                Debug.Log("Set dialogue to state 2");
            }
            else //if the dialogue is 2, resets the dialogue, used to test if both controller and keyboard are working as intended
            {
                setDialogue(0);
                Debug.Log("Reset Dialogue state");
            }
            
        }**/
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

    public void setDialogue(int x) //used to change the dialogue state
    {
        dialogueState = x;
    }

}
