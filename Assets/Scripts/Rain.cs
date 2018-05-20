using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour {

	public GameObject cloud;
	public GameObject grass;
	public Material gray;
	Vector3 spot;

	void Start()
	{
		spot = grass.transform.position;
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("collision");
		this.StartCoroutine (storm());
	}

	IEnumerator storm()
	{
		yield return new WaitForSeconds (1);
		cloud.GetComponent<MeshRenderer> ().material = gray;
		yield return new WaitForSeconds (2);
		while (grass.transform.position.y < -86.7) 
		{
			spot.y += 1;
			grass.transform.position = spot;
			yield return new WaitForEndOfFrame ();
		}
		Destroy (this.gameObject);
	}
}
