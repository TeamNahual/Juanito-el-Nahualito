using UnityEngine;
using System.Collections;

public class DialogueTrigger : MonoBehaviour {
	public bool deleteOnEnter = false;
	public string[] messages;
	public int[] timers;
	public AudioClip[] audioClips;
	public bool[] locksMovement;
	
	void Awake()
	{
		// Hide the component in game
		GetComponent<Renderer>().enabled = false;
	}
	
    void OnTriggerEnter(Collider other) {

    	if(other.gameObject != Juanito.ins.JuanitoHuman)
    		return;
    	
		UIManager.instance.dialogueSystem.addDialogue(
			messages, timers, audioClips, locksMovement);
		if (deleteOnEnter) {
			Destroy(gameObject);
		}
    }
}
