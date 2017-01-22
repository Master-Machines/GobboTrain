using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameCanvasScript : MonoBehaviour {

	public Text scoreText;

	// Use this for initialization
	void Start () {
		scoreText.text = "Score: " + Global.Instance.lastScore;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BackButtonClicked()
	{
		Application.LoadLevel(0);
	}

}
