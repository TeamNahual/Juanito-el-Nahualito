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
		
		Renderer rend = GetComponent<Renderer>();
        rend.material.SetFloat("_AnimOffset", Random.value * 10.0f);
		
		float lowerBound = 10.0f;
		float upperBound = 15.0f;
		
		randVector = new Vector3(
			(Random.value > 0.5)? Random.Range(lowerBound, upperBound): Random.Range(-upperBound, -lowerBound),
			(Random.value > 0.5)? Random.Range(lowerBound, upperBound): Random.Range(-upperBound, -lowerBound),
			(Random.value > 0.5)? Random.Range(lowerBound, upperBound): Random.Range(-upperBound, -lowerBound)
		);
    }

    void Update()
    {
		// Calculate next position
		float time = (Time.time / randVel + randOffset) / 50.0f;
		Vector3 nextPosition = new Vector3(
			origin.x + Mathf.Sin( (origin.x + randVector.x) * time ),
			origin.y + Mathf.Cos( (origin.y + randVector.y) * time ) + 2,
			origin.z + Mathf.Cos( (origin.z + randVector.z) * time )
		);

        // Move and Rotate butterflies
		transform.rotation = Quaternion.LookRotation(nextPosition - transform.position);
        transform.position = nextPosition;
    }
}
