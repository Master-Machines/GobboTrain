using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameCanvasScript : MonoBehaviour {

	public Text scoreText;
    public Text goldText;
    public Text newHighScore;

    private GameController GameController;

	// Use this for initialization
	void Start () {
		scoreText.text = "Last Run: " + Global.Instance.lastScore;
        goldText.text = Global.Instance.highScore + " :Best Run";
        newHighScore.text = "";


    }

    public void newHighness()
    {
        if (GameController.Score == Global.Instance.highScore)
        {
            newHighScore.text = "NEW HIGH SCORE!!!!";
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
