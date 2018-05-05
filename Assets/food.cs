using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

    public static Food ins;

    public GameObject fud;

    void Awake()
    {
        ins = this;
    }

    // Use this for initialization
   void Start () {

    }

    // Update is called once per frame
    void Update () {
    }
}