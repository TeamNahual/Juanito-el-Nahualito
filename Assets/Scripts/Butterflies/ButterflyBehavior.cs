using UnityEngine;
using System.Collections;

public class ButterflyBehavior : MonoBehaviour
{
    // Reference to the controller.
    public ButterflySpawner controller;

	// Origin of orbit
	public Vector3 origin;
	
	// Velocity
	public float velBase = 10.0f;
	public float velVariation = 10.0f;
	
    // Randoms
	Vector3 randVector;
    float randVel;
    float randOffset;

    void Start()
    {
        randOffset = Random.value * 50.0f;
		
		randVel = Random.Range(velBase - velVariation, velBase + velVariation);
		Debug.Log(randVel);
		
		Renderer rend = GetComponent<Renderer>();
        rend.material.SetFloat("_AnimOffset", Random.value * 10.0f);
		
		randVector = new Vector3(
			(Random.value > 0.5)? Random.Range(2.5f, 5.0f): Random.Range(-5.0f, -2.5f),
			(Random.value > 0.5)? Random.Range(2.5f, 5.0f): Random.Range(-5.0f, -2.5f),
			(Random.value > 0.5)? Random.Range(2.5f, 5.0f): Random.Range(-5.0f, -2.5f)
		);
    }

    void Update()
    {
		// Calculate next position
		var time = Time.time / randVel + randOffset;
		var nextPosition = new Vector3(
			Mathf.Sin( (origin.x + randVector.x) * time ),
			Mathf.Cos( (origin.y + randVector.y) * time ) + 1,
			Mathf.Cos( (origin.z + randVector.z) * time )
		);

        // Move and Rotate butterflies
		transform.rotation = Quaternion.LookRotation(nextPosition - transform.position);
        transform.position = nextPosition;
    }
}
