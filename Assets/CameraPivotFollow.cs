using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivotFollow : MonoBehaviour {

    public Transform player;
  

    public static CameraPivotFollow ins;

    void Awake()
    {
        ins = this;
    }

    void Start()
    {
        // sets the camera pivot target to the player
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        // move camera pilot to player
        transform.position = player.position;
        transform.rotation = player.rotation;
        //Heading.LookAt(targetObjective);
        
    }
}
