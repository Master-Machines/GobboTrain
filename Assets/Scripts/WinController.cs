using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinController : MonoBehaviour {

	public GameController GameController;
	public Text ScoreDisplay;

	public void Continue() {
		Application.LoadLevel("GameScene");
	}

	public void Display() {
		ScoreDisplay.text = "SCORE: " + GameController.Score;
	}
}
