using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour {

	public GameObject grass;
	public Material gray;

    public ParticleSystem spiritTrails;
	public AudioSource sound;

    public ParticleSystem grassParticles;
    public ParticleSystem foliageParticles;

	void Start()
	{
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("collision");
		this.StartCoroutine (storm());
		Renderer show = GetComponent<Renderer> ();
		show.enabled = false;
	}

	IEnumerator storm()
	{
        spiritTrails.Play();
		yield return new WaitForSeconds (1);
		sound.Play ();
        grassParticles.Play();
        foliageParticles.Play();
        yield return new WaitForSeconds (2);

		Destroy (this.gameObject);
	}
}
