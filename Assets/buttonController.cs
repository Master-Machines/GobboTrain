using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonController : MonoBehaviour {

    public void playButton(string levelName)
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);

    }

    public void howToButton(string levelName)
    {
        SceneManager.LoadScene("howToPlayScene", LoadSceneMode.Single);
    }

    public void storeButton(string levelName)
    {
        SceneManager.LoadScene("Store", LoadSceneMode.Single);
    }

    public void creditsButton(string levelName)
    {
        SceneManager.LoadScene("creditsScene", LoadSceneMode.Single);
    }

    public void returnToMainMenu(string levelName)
    {
        SceneManager.LoadScene("startScreen", LoadSceneMode.Single);
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
       
    }

}
