using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using TMPro;

public class SpeechBubble : MonoBehaviour {

	public GameObject package;

	GameManager manager;
	int counter = 0;
	string tip = "Press <sprite=0> to talk";

	//use the setDialogue function to change this value.
	[SerializeField]
	private int dialogueState = 0; //0 is never talked to, 1 is talked to once, 2 is talked to and done quest

	//dialogue for NPC, public so we can tweak it without going into code.
	public string[] firstDia; // first thing npc says
	public string[] secondDia; //thing npc says after talked to once
	public string[] thirdDia; //thing npc says after completed task

	public TextMeshProUGUI speech;

	void Awake(){
		package.SetActive (false);
	}

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
			UIManager.instance.TooltipDisplay(tip); //displays message saying press X to talk
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
				manager.isMovementLocked = true;
				if (counter < firstDia.Length)
				{
					speech.text = firstDia[counter];
					speech.enabled = true;
					counter++;
					tip = "Press <sprite=0> to continue";
				}
				else
				{
					dialogueState = 1; //player has talked to NPC
					counter = 0;
					manager.isMovementLocked = false;
					speech.enabled = false;
					tip = "Press <sprite=0> to talk";
				}
				break;
			case 1: //state when player has talked to NPC once, haven't completed quest yet
				package.SetActive (true);
				manager.isMovementLocked = true;
				if(counter <secondDia.Length)
				{
					speech.text = secondDia[counter];
					speech.enabled = true;
					counter++;
					tip = "Press <sprite=0> to continue";
				}
				else
				{
					manager.isMovementLocked = false;
					speech.enabled = false;
					counter = 0;
					tip = "Press <sprite=0> to talk";
                        Juanito.ins.JuanitoHuman.transform.position = new Vector3(173.5135f, 90.44999f, 262.44f);
				}
				break;
			case 2: //player has completed quest
				manager.isMovementLocked = true;
				if(counter <thirdDia.Length)
				{
					speech.text = thirdDia[counter];
					speech.enabled = true;
					counter++;
					tip = "Press <sprite=0> to continue";
				}
				else
				{
					manager.isMovementLocked = false;
					speech.enabled = false;
					counter = 0;
					tip = "Press <sprite=0> to talk";
				}
				break;
			default:
				Debug.Log("You broke my code.");
				break;
			}
		}
	}

	void OnTriggerExit(Collider other) //when an object moves out of box
	{
		if (other.gameObject == Juanito.ins.JuanitoHuman)//if object is Juanito clear the tool tip message
		{
			UIManager.instance.TooltipDisable(); //turns off the tooltips
			UIManager.instance.toggleDialogueBox(false); //turns off the dialogue boxes
			speech.enabled = false;
			//closes the dialogue box
		}
	}

	public void setDialogue(int x) //used to change the dialogue state
	{
		dialogueState = x;
	}

	// Use this for initialization
	void Start () {
		speech.enabled = false;
		manager = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
		if (speech.enabled) 
		{
			speech.transform.LookAt (Camera.main.transform);
		}
	}
}
