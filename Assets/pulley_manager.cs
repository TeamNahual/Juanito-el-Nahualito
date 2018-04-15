using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulley_manager : MonoBehaviour {

	public pulley_platform plat1, plat2;

	private Vector3 topPos, bottomPos;

	// Use this for initialization
	void Start () {
		if (plat1.transform.position.y < plat2.transform.position.y) {
			topPos = plat2.transform.position;
			bottomPos = plat1.transform.position;
		} else {
			topPos = plat1.transform.position;
			bottomPos = plat2.transform.position;
		}
	}
	
	public void MovePlatforms(){
		float w1 = plat1.GetWeight ();
		float w2 = plat2.GetWeight ();
		if (w1 < w2) {
			plat1.MoveTo (topPos);
			plat2.MoveTo (bottomPos);
		} else if (w1 > w2) {
			plat1.MoveTo (bottomPos);
			plat2.MoveTo (topPos);
		}
	}
}
