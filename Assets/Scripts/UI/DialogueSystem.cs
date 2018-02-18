using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {
	public string text;
	public int timer;
	public AudioClip audioClip;
	public bool isMovementLocked;
	
	public Dialogue(string diaText, int diaTimer, AudioClip diaAudio, bool movementLocked) {
		text = diaText;
		timer = diaTimer;
		audioClip = diaAudio;
		isMovementLocked = movementLocked;
	}
	
	public Dialogue(string diaText) : this(diaText, 0, null, false) {}
}

public class DialogueSystem : MonoBehaviour
{
	// Dialogue System
	private Queue dialogueQueue;
	private int timerEnd;

	public DialogueSystem() {
		dialogueQueue = new Queue();
		timerEnd = -1;
	}
	
	public void Update()
	{
		if (timerEnd > 0 && (int) (Time.time * 1000) > timerEnd) {
			timerEnd = -1;
			doDialogueAction();
		}
	}
	
	private void updateDialogueText()
	{
		bool isDialogueOpen = dialogueQueue.Count > 0;
		if (isDialogueOpen) {
			Dialogue dialogue = (Dialogue) dialogueQueue.Peek();
			GameManager.instance.isMovementLocked = dialogue.isMovementLocked;
			UIManager.instance.setDialogueBoxText(dialogue.text);
			if (dialogue.timer > 0) {
				timerEnd = (int) (Time.time * 1000) + dialogue.timer;
			}
			if (dialogue.audioClip != null) {
				UIManager.instance.setAndPlayAudioClip(dialogue.audioClip);
			}
		} else {
			GameManager.instance.isMovementLocked = false;
		}
		UIManager.instance.toggleDialogueBox(isDialogueOpen);
	}
	
	public void doDialogueAction()
	{
		if (dialogueQueue.Count > 0) {
			dialogueQueue.Dequeue();
		}
		updateDialogueText();
	}
	
	
	public void addDialogue(Dialogue dialogue) {
		dialogueQueue.Enqueue(dialogue);
		updateDialogueText();
	}
	
	public void addDialogue(Dialogue[] dialogue) {
		for (int i = 0; i < dialogue.Length; ++i) {
			dialogueQueue.Enqueue(dialogue[i]);
		}
		updateDialogueText();
	}
	
	public void addDialogue(string[] texts, int[] timers, AudioClip[] audioClips, bool[] movementLocked) {
		for (int i = 0; i < texts.Length; ++i) {
			string text = texts[i];
			int timer = (i < timers.Length)? timers[i]: 0;
			AudioClip audioClip = (i < audioClips.Length)? audioClips[i]: null;
			bool isMovementLocked = !(i >= movementLocked.Length || !movementLocked[i]);
			dialogueQueue.Enqueue(new Dialogue(text, timer, audioClip, isMovementLocked));
		}
		updateDialogueText();
	}
	
	// Overloaded functions
	public void addDialogue(string text) {
		this.addDialogue(new Dialogue(text));
	}
	public void addDialogue(string text, int timer, AudioClip audioClip, bool movementLocked) {
		this.addDialogue(new Dialogue(text, timer, audioClip, movementLocked));
	}
	public void addDialogue(string[] texts) {
		this.addDialogue(texts, new int[0], new AudioClip[0], new bool[0]);
	}
}
