using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyPuzzleManager : MonoBehaviour {

	public List<GameObject> pieces;
	public List<GameObject> piecesStatic;
	public bool allActive = false;
	public RelicPedestal relic;
	public GameObject VisibleInSpiritMode;
	public bool completed = false;
	public GameObject pyramidDoor;
	public bool doorOpened = false;

	public static ButterflyPuzzleManager ins;

	private bool ShowBlocks = true;

	// Use this for initialization
	void Start () {

		ins = this;

		foreach(GameObject piece in pieces)
		{
			piece.SetActive(false);
		}

		NextPiece();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		ManageBlockSpiritState();
		ManageDoor();
	}

	public void CheckPieces()
	{
		if(pieces.Count == 0)
		{
			Debug.Log ("You have completed the Butterfly Puzzle");
			StartCoroutine(relic.RaisePedestal());
			completed = true;
		}
		else
		{
			NextPiece();
		}
	}

	void ManageDoor()
	{
		if(!completed)
			return; 

		if(!relic.active && !doorOpened)
			StartCoroutine(OpenDoor());
	}

	IEnumerator OpenDoor()
	{
		doorOpened = true;

		Transform doorLeft = pyramidDoor.transform.GetChild(0);
		Vector3 LStart = doorLeft.localPosition;
		Vector3 LEnd = new Vector3(LStart.x, LStart.y, -2.5f);

		Transform doorRight = pyramidDoor.transform.GetChild(1);
		Vector3 RStart = doorRight.localPosition;
		Vector3 REnd = new Vector3(RStart.x, RStart.y, 4.2f);

		float k = 0;

		while (k < 2) 
		{
			doorLeft.localPosition = Vector3.Lerp(LStart, LEnd, k);
			doorRight.localPosition = Vector3.Lerp(RStart, REnd, k);

			k += Time.deltaTime * Random.Range(0.5f,1f);
			yield return null;
		}

	}

	void NextPiece()
	{
		int randomIndex = Random.Range(0, pieces.Count);

		GameObject currPiece = pieces[randomIndex];

		if((currPiece.name == "Piece_TopLeft" 
				&& piecesStatic.Find(obj => obj.name == "Piece_MiddleLeft") == null)
			|| (currPiece.name == "Piece_TopRight" 
				&& piecesStatic.Find(obj => obj.name == "Piece_MiddleRight") == null))
		{
			NextPiece();
			return;
		}

		piecesStatic.Add(currPiece);

		pieces.RemoveAt(randomIndex);
		
		currPiece.SetActive(true);
	}

	void ManageBlockSpiritState()
	{
		if(Juanito.ins.SpiritState)
		{
			if(!ShowBlocks)
			{
				ShowBlocks = true;

				VisibleInSpiritMode.SetActive(true);

				foreach(GameObject piece in piecesStatic)
				{
					piece.SetActive(true);
				}
			}
		}
		else
		{
			if(ShowBlocks)
			{
				ShowBlocks = false;

				VisibleInSpiritMode.SetActive(false);

				foreach(GameObject piece in piecesStatic)
				{
					piece.SetActive(false);
				}
			}
		}
	}
}
