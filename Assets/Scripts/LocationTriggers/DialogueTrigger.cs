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
		string START_COLOR = "<color=#facadeff>";
		string END_COLOR = "</color>";
		
		string output = "";
		
		string[] messageTokens = message.Split(' ');
		string[] colorCodeTokens = colorCode.Split(' ');
		int c = 0;
		string space = "";
		bool color = false;
		
		for (int i = 0; i < messageTokens.Length; ++i) {
			string code = "";
			if (c < colorCodeTokens.Length && messageTokens[i] == colorCodeTokens[c]) {
				if (!color) {
					color = true;
					code = START_COLOR;
				}
				++c;
			} else {
				if (color) {
					color = false;
					code = END_COLOR;
				}
			}
			output += space + code + messageTokens[i];
			space = " ";
		}
		return output;
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
