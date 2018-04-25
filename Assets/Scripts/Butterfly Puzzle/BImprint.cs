using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BImprint : MonoBehaviour {

	public GameObject desiredPiece;
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
		if (collider.transform.parent.gameObject == desiredPiece) {
			Vector3 pos1 = new Vector3 (transform.position.x, 0, transform.position.z);
			Vector3 pos2 = new Vector3 (desiredPiece.transform.position.x, 0, desiredPiece.transform.position.z);

			currentDistance = Vector3.Distance (pos1, pos2);

			if (Vector3.Distance (pos1, pos2) < relief) {

				desiredPiece.GetComponent<ButterflyBodyV2> ().DetachPlayer ();
				desiredPiece.transform.localPosition = Vector3.zero;
				desiredPiece.GetComponent<ButterflyBodyV2> ().locked = true;
				desiredPiece.GetComponent<Rigidbody>().isKinematic = true;
				box.enabled = false;

				ButterflyPuzzleManager.ins.CheckPieces ();

			}
		}
	}
}
