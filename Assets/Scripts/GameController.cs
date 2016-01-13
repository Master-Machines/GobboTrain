using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {
	public Text ScoreDisplay;
	public Text CurrencyDisplay;
	public Text PowerDisplay;
	//public Text SpeedDisplay;
	public Text MultiplierDisplay;
	public Text NewPointsDisplay;
	public Text WallWarning;

	// Modifies the required momentum.
	public const float DifficultyModifier = 1.5f;

	public int Score {get;set;}
	public WinController WinController;
	public PlayerController PlayerController;
	public int Multiplier = 1;
	public int SessionCurrency = 0;
	public float TimePLayed = 0f;
	public Button PauseButton;

	public ObstacleCreator ObstacleCreator;


	void Start() {
//		Pause ();
//		HideUI(false);
//		UnPause();
//		HideUI (true);
//		DialogGenerator.CreateCustomDialog("StartGameDialog", new Vector2(0f, 1200f), null, (int result)=> {
//			
//		});
	}

	public void IncreaseCurrency(int amount) {
		SessionCurrency += amount;
		CurrencyDisplay.text = SessionCurrency.ToString() + " G"; 
		NewPointsDisplay.text = "+" + (amount).ToString() + " G";
		CancelInvoke("HideNewPoints");
		Invoke ("HideNewPoints", .5f);
	}

	public void IncreaseScore(int amount) {
		Score += amount * Multiplier;
		ScoreDisplay.text = string.Format("{0:#,##0}", Score); 
	}

	void HideNewPoints() {
		NewPointsDisplay.text = "";
	}

	public void IncreaseMultiplier() {
		Multiplier ++;
		MultiplierDisplay.text = Multiplier.ToString() + "x";
		MultiplierDisplay.color = new Color(0f, 1f, 0f);
		CancelInvoke("UnhighlightMultiplier");
		Invoke("UnhighlightMultiplier", .5f);
	}

	void UnhighlightMultiplier() {
		MultiplierDisplay.color = Color.white;
	}

	public void GameOver() {
		DialogGenerator.CreateCustomDialog("GameOverDialog", new Vector2(-1000, 0), (GameObject dialog)=>{
			dialog.GetComponent<GameOverDialog>().Setup(this);
			HideUI(false);
		}, (int result)=>{
			StartCoroutine(DelayedNewGame());
		});
	}

	void HideUI(bool show) {
		ScoreDisplay.gameObject.SetActive(show);
		//SpeedDisplay.gameObject.SetActive(show);
		MultiplierDisplay.gameObject.SetActive(show);
		NewPointsDisplay.gameObject.SetActive(show);
		WallWarning.gameObject.SetActive(false);
		CurrencyDisplay.gameObject.SetActive(show);
		PauseButton.gameObject.SetActive(show);
	}

	IEnumerator DelayedNewGame() {
		yield return new WaitForSeconds(.6f);
		Application.LoadLevel("GameScene");
	}

	public void Win() {
		WinController.gameObject.SetActive(true);
		WinController.Display();
	}

	public void DisplayWallWarning() {
		//WallWarning.gameObject.SetActive(true);
		//CancelInvoke("WallWarningOff");
		//Invoke ("WallWarningOff", 4f);
	}

	public void Pause() {
		TimeController.Pause (true);
	}

	public void UnPause() {
		TimeController.Pause(false);
	}

	void WallWarningOff() {
		WallWarning.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		UpdateText();
		TimePLayed += Time.deltaTime;
	}

	void UpdateText() {
		float power = 10f + (float)PlayerController.MomentumBonuses + (10f * (float)PlayerController.CurrentGoblins * PlayerController.GoblinPowerBonus);
		string powerString = (power/10f).ToString();
		if(PlayerController.MomentumBonuses % 10 > 0) {
			powerString = powerString.Substring(0, powerString.IndexOf('.') + 2);
		}

		int speed = (int)PlayerController.GetSpeed();
		//SpeedDisplay.text = "SPEED: " + speed.ToString();
	}
}
