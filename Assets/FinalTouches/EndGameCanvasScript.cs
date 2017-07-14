using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameCanvasScript : MonoBehaviour {

	public Text scoreText;
    public Text goldText;
    public Image newHighScore;

    private GameController GameController;

	// Use this for initialization
	void Start () {
		scoreText.text = "Last Run: " + Global.Instance.lastScore;
        goldText.text = Global.Instance.highScore + " :Best Run";
        newHighScore.enabled = false;
        if(Global.Instance.lastScore == Global.Instance.highScore)
        {
            newHighScore.enabled = true;
        }
    }
    
	// Update is called once per frame
	void Update () {
	
	}

	public void BackButtonClicked()
	{
		Application.LoadLevel(0);
	}

}
