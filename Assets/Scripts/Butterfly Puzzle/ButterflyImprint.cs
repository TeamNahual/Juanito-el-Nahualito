using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyImprint : MonoBehaviour {

	public GameObject desiredPiece;
	public ButterflyPuzzle puzzle;
	public float relief = 0.1f;
	public BoxCollider box;
	public float currentDistance;

	// Use this for initialization
	void Start () {
		box = GetComponent<BoxCollider> ();
		box.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider collider)
	{
		if (collider.gameObject == desiredPiece) {
			Vector3 pos1 = new Vector3 (transform.position.x, 0, transform.position.z);
			Vector3 pos2 = new Vector3 (desiredPiece.transform.position.x, 0, desiredPiece.transform.position.z);

			currentDistance = Vector3.Distance (pos1, pos2);

			if (Vector3.Distance (pos1, pos2) < relief) {
				desiredPiece.transform.parent.position = new Vector3 (transform.position.x, desiredPiece.transform.parent.position.y, transform.position.z);
				desiredPiece.GetComponent<ButterflyMesh> ().main.locked = true;
				desiredPiece.GetComponent<ButterflyMesh> ().main.DetachPlayer ();
				box.enabled = false;

				if (puzzle.CheckPieces ()) 
				{
					Debug.Log ("You have completed the Butterfly Puzzle");	
				} else 
				{

				}
			}
		}
	}
}
