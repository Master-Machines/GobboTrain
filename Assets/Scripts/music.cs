using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicController: MonoBehaviour {

    public AudioSource _AudioSource1;
    public AudioSource _AudioSource2;

    public TimeController TimeController;


    // Use this for initialization
    void Start () {

        _AudioSource1.loop = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
