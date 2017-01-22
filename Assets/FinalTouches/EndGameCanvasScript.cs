using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameCanvasScript : MonoBehaviour {

	public Text scoreText;
    public Text goldText;

	// Use this for initialization
	void Start () {
		scoreText.text = "Score: " + Global.Instance.lastScore;
        goldText.text = "Gold: " + Global.Instance.SessionGold;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BackButtonClicked()
	{
		Application.LoadLevel(0);
	}

}
