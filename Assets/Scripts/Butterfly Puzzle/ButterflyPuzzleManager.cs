using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyPuzzleManager : MonoBehaviour {

	public List<GameObject> pieces;
	public List<GameObject> piecesStatic;

	private bool ShowBlocks = true;

	// Use this for initialization
	void Start () {

		// piecesStatic = pieces;

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

	void NextPiece()
	{
		int randomIndex = Random.Range(0, pieces.Count);

		GameObject currPiece = pieces[randomIndex];

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
