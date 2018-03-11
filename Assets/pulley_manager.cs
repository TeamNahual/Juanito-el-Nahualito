using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulley_manager : MonoBehaviour {

	public Transform top, mid, bottom;
	public pulley_platform plat1, plat2;

	private Vector3 topPos, midPos, bottomPos;

	// Use this for initialization
	void Start () {
		topPos = top.position;
		midPos = mid.position;
		bottomPos = bottom.position;
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
		} else {
			plat1.MoveTo (midPos);
			plat2.MoveTo (midPos);
		}
	}
}
