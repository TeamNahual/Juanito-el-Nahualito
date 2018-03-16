using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleToTarget : MonoBehaviour {

    public ParticleSystem p;
    public ParticleSystem.Particle[] particles;
    public Transform Target;
    public float affectDistance;
    float sqrDist;
    Transform thisTransform;
    Vector3 offset = new Vector3(0, 0.5f, 0);


    void Start()
    {
        p = GetComponent<ParticleSystem>();
        sqrDist = affectDistance * affectDistance;
    }
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Animal");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
    private void FixedUpdate()
    {
        if(FindClosestEnemy() != null)
        {
            Target = FindClosestEnemy().transform;
        }
    }
    void Update()
    {

        particles = new ParticleSystem.Particle[p.particleCount];

        p.GetParticles(particles);

        for (int i = 0; i < particles.GetUpperBound(0); i++)
        {

            float ForceToAdd = (particles[i].startLifetime - particles[i].remainingLifetime) * (0.2f * Vector3.Distance(Target.position, particles[i].position));

            //Debug.DrawRay (particles [i].position, (Target.position - particles [i].position).normalized * (ForceToAdd/10));

            particles[i].velocity = (Target.position - particles[i].position).normalized * ForceToAdd;

            //particles [i].position = Vector3.Lerp (particles [i].position, Target.position, Time.deltaTime / 2.0f);

        }

        p.SetParticles(particles, particles.Length);

    }
}
