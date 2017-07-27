using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour {
	public Text ScoreDisplay;
	public Text CurrencyDisplay;
	public Text PowerDisplay;
	//public Text SpeedDisplay;
	public Text MultiplierDisplay;
	public Text NewPointsDisplay;
	public Text WallWarning;
	public Image powerbar;

	// Modifies the required momentum.
	public const float DifficultyModifier = 1.5f;

	public int Score {get;set;}
	public WinController WinController;
	public PlayerController PlayerController;
	public int Multiplier = 1;
	public int SessionCurrency = 0;
	public float TimePLayed = 0f;
	public Button PauseButton;

	public Sprite powerBarZero, powerBarFifty, powerBarSeventyFive, powerBarNinetyNine, powerBarFull;

	public ObstacleCreator ObstacleCreator;

	public GameObject EndGameCanvas;

    private void Awake()
    {
        Global.Instance.highScore = PlayerPrefs.GetInt("highScore");
    }

    void Start() {
		Global.Instance.lastScore = 0;
        CurrencyDisplay.text = "Best Run: " + Global.Instance.highScore;
        /*Pause ();
		HideUI(false);
		DialogGenerator.CreateCustomDialog("StartGameDialog", new Vector2(0f, 1200f), null, (int result)=> {
			UnPause();
			HideUI (true);
		});*/
    }

	public void IncreaseCurrency(int amount) {
        /*SessionCurrency += amount;
        CurrencyDisplay.text = "High Score: " + Global.Instance.highScore; 
		NewPointsDisplay.text = "+" + (amount).ToString() + " G";
		CancelInvoke("HideNewPoints");
		Invoke ("HideNewPoints", .5f);*/
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
		Global.Instance.lastScore = Score;
        if (Global.Instance.highScore<Score)
            {
            Global.Instance.highScore = Score;
            }
        Global.Instance.Save();
		SceneManager.LoadScene("EndScreen", LoadSceneMode.Single);
	}

	void HideUI(bool show) {
		ScoreDisplay.gameObject.SetActive(show);
		//SpeedDisplay.gameObject.SetActive(show);
		MultiplierDisplay.gameObject.SetActive(show);
		NewPointsDisplay.gameObject.SetActive(show);
		WallWarning.gameObject.SetActive(false);
		CurrencyDisplay.gameObject.SetActive(show);
		PauseButton.gameObject.SetActive(show);
		powerbar.gameObject.SetActive(show);
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
		/*UpdatePowerBar();*/
		TimePLayed += Time.deltaTime;
	}

	// AUSTIN LOOK HERE
	/*void UpdatePowerBar()
	{
		//PlayerController.DetermineMomentum();
		//GameObject.Find("LevelController").GetComponent<LevelController>().WallHealth;
		float powerBarPercent = (PlayerController.DetermineMomentum() / GameObject.Find("LevelController").GetComponent<LevelController>().WallHealth) * 100; 
		if(powerBarPercent < 25)
		{
			//powerBarNull sprite
			powerbar.sprite = powerBarZero;
			powerbar.rectTransform.sizeDelta = new Vector2(powerBarPercent * 4, 40);
		}else if(powerBarPercent < 50)
		{
			//powerball 25 sprite
			powerbar.sprite = powerBarFifty;
			powerbar.rectTransform.sizeDelta = new Vector2(powerBarPercent * 4, 40);
		}else if(powerBarPercent < 75)
		{
			//powerbar 50 sprite
			powerbar.sprite = powerBarSeventyFive;
			powerbar.rectTransform.sizeDelta = new Vector2(powerBarPercent * 4, 40);
		}else if(powerBarPercent < 100)
		{
			//powerbar 99 sprite
			powerbar.sprite = powerBarNinetyNine;
			powerbar.rectTransform.sizeDelta = new Vector2(powerBarPercent * 4, 40);
		}else if(powerBarPercent >= 100)
		{
			//powerbar 100 sprite
			powerbar.sprite = powerBarFull;
			powerbar.rectTransform.sizeDelta = new Vector2(400, 40);
		}
		
	}*/

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
