using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameOverDialog : DialogController {
	public Text CurrencyDisplay;
	public Text ScoreBonus;
	public Text Score;
	public Text HighScore;
	public Text TotalCurrency;

	private GameController GameController;

	public void Setup(GameController GameController) {
		this.GameController = GameController;
		int speed = (int)GameController.PlayerController.ImpactSpeed;
		int time = (int)GameController.TimePLayed;
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)time);
		Score.text = string.Format("{0:#,##0}", GameController.Score); 
		CurrencyDisplay.text = "Destruction: " + GameController.SessionCurrency.ToString() + "c";
		Global.Instance.currency += GameController.SessionCurrency;
		if(Global.Instance.highScore < GameController.Score) {
			Global.Instance.highScore = GameController.Score;
			HighScore.gameObject.SetActive(true);
		}

		DetermineCurrencyBonus();

		TotalCurrency.text = Global.Instance.currency.ToString() + "c";

		Global.Instance.Save();
	}

	void DetermineCurrencyBonus() {
		int bonus = (int)(GameController.Score / 2000);
		ScoreBonus.text = "Score Bonus: " + bonus.ToString() + "c";
		Global.Instance.currency += bonus;
	}
}
