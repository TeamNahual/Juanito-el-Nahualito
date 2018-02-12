using UnityEngine;
using System.Collections;

public class DialogueTrigger : MonoBehaviour {
	public bool deleteOnEnter = false;
	public string[] dialogueStrings;
	public AudioClip[] dialogueAudio;
	public int[] dialogueTimers;
	
	void Awake()
	{
		// Hide the component in game
		GetComponent<Renderer>().enabled = false;
	}
	
    void OnTriggerEnter(Collider other) {
		UIManager.instance.addDialogue(dialogueStrings,
			dialogueTimers, dialogueAudio);
		if (deleteOnEnter) {
			Destroy(gameObject);
		}
    }
}
