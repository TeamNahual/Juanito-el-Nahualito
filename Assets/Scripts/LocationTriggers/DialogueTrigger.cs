using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class DialogueTrigger : MonoBehaviour {
	public bool deleteOnEnter = false;
	public string[] messages;
	public string[] colorCoded;
	public int[] timers;
	public AudioClip[] audioClips;
	public bool[] locksMovement;

	public AudioMixerSnapshot voiceOver;
	
	void Awake()
	{
		// Hide the component in game
		GetComponent<Renderer>().enabled = false;
	}
	
	string colorCodeString(string message, string colorCode) {
		string START_SPANISH = "<color=#60c5acff>";
		string START_ENGLISH = "<color=#cfb76fff>";
		string END_COLOR = "</color>";
		
		string output = "";
		
		string[] messageTokens = message.Split(' ');
		string[] colorCodeTokens = colorCode.Split(' ');
		int c = 0;
		string space = "";
		bool spanish = false;
		
		for (int i = 0; i < messageTokens.Length; ++i) {
			string code = "";
			if (c < colorCodeTokens.Length && messageTokens[i] == colorCodeTokens[c]) {
				if (!spanish) {
					spanish = true;
					if (space == "") {
						code = START_SPANISH;
					} else {
						code = END_COLOR + START_SPANISH;
					}
				}
				++c;
			} else {
				if (spanish) {
					spanish = false;
					code = END_COLOR + START_ENGLISH;
				}
			}
			
			// In case the first string isn't in Spanish
			if (space == "" && !spanish) {
				space = "" + START_ENGLISH;
			}
			output += space + code + messageTokens[i];
			space = " ";
		}
		return output + END_COLOR;
	}
	
    void OnTriggerEnter(Collider other) {

    	if(other.gameObject != Juanito.ins.JuanitoHuman)
    		return;

		voiceOver.TransitionTo (.25f);
		//Debug.Log ("Should have Transitioned");

    	if(other.gameObject != Juanito.ins.JuanitoHuman)
    		return;

		string[] modifiedMessages = new string[messages.Length];
		for (int i = 0; i < messages.Length; ++i) {
			modifiedMessages[i] = colorCodeString(messages[i],
				(i < colorCoded.Length)? colorCoded[i]: "");
		}
		UIManager.instance.dialogueSystem.addDialogue(
			modifiedMessages, timers, audioClips, locksMovement);
		if (deleteOnEnter) {
			Destroy(gameObject);
		}
    }
}
