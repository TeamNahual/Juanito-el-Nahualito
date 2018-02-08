using UnityEngine;
using System.Collections;

public class DialogueTrigger : MonoBehaviour {
	public bool deleteOnEnter = false;
	public string[] dialogue;
	
	void Awake()
	{
		// Hide the component in game
		GetComponent<Renderer>().enabled = false;
	}
	
    void OnTriggerEnter(Collider other) {
		UIManager.instance.addDialogueString(dialogue);
		if (deleteOnEnter) {
			Destroy(gameObject);
		}
    }
}
