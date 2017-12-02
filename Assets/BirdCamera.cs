using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BirdCamera : MonoBehaviour {
    public CinemachineVirtualCamera cam1;
    public CinemachineBlendListCamera cam2;
    public bool switcher = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        switcher = !switcher;
        if (!switcher)
        {
            cam1.Priority = 1;
            cam2.Priority = 10;
        }
        else
        {
            cam1.Priority = 10;
            cam2.Priority = 1;
        }
    }
}
