﻿using UnityEngine;
using System.Collections;

public class ButterflyBehavior : MonoBehaviour
{
    // Reference to the controller.
    public ButterflySpawner controller;

	// Origin of orbit
	public Vector3 origin;
	public float spawnRadius = 2.0f;
	public bool followJuanito = false;
	
	// Velocity
	public float velBase = 5.0f;
	public float velVariation = 5.0f;
	
    // Randoms
	Vector3 randVector;
    float randVel;
    float randOffset;

    void Start()
    {
        randOffset = Random.value * 50.0f;
		
		randVel = Random.Range(velBase - velVariation, velBase + velVariation) / spawnRadius;
		
		Renderer rend = GetComponent<Renderer>();
        rend.material.SetFloat("_AnimOffset", Random.value * 10.0f);
		
		float lowerBound = spawnRadius;
		float upperBound = lowerBound + (lowerBound / 2);
		
		randVector = new Vector3(
			(Random.value > 0.5)? Random.Range(lowerBound, upperBound): Random.Range(-upperBound, -lowerBound),
			(Random.value > 0.5)? Random.Range(lowerBound, upperBound): Random.Range(-upperBound, -lowerBound),
			(Random.value > 0.5)? Random.Range(lowerBound, upperBound): Random.Range(-upperBound, -lowerBound)
		);
    }

    void Update()
    {
		Vector3 center = (!followJuanito)? origin:
			Juanito.ins.JuanitoSpirit.transform.position;
			
		float speedDiv = (followJuanito)? 100.0f: 200.0f;
		
		// Calculate next position
		float time = (Time.time / randVel + randOffset) / speedDiv;
		Vector3 nextPosition = new Vector3(
			center.x +  Mathf.Sin( (origin.x + randVector.x) * time ) * spawnRadius,
			center.y + (Mathf.Cos( (origin.y + randVector.y) * time ) + 2) * spawnRadius / 2,
			center.z +  Mathf.Cos( (origin.z + randVector.z) * time ) * spawnRadius
		);

        // Move and Rotate butterflies
		transform.rotation = Quaternion.LookRotation(nextPosition - transform.position);
        transform.position = nextPosition;
    }
}
