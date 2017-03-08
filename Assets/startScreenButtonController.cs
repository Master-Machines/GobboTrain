using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startScreenButtonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
