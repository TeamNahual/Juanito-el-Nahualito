using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyPuzzleManager : MonoBehaviour {

	public List<GameObject> pieces;
	public List<GameObject> piecesStatic;
	public bool allActive = false;
	public RelicPedestal relic;

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
	void Update () {
		ManageBlockSpiritState();
	}

	public void CheckPieces()
	{
		if(pieces.Count == 0)
		{
			Debug.Log ("You have completed the Butterfly Puzzle");
			StartCoroutine(relic.RaisePedestal());
		}
		else
		{
			NextPiece();
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

				foreach(GameObject piece in piecesStatic)
				{
					piece.SetActive(false);
				}
			}
		}
	}
}
