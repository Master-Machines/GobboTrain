using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeController : MonoBehaviour {
	public static bool IsPaused {get; private set;}
	public static float EnterSlowMotionPosition;
	public static bool WallDestroyed = false;


	public static bool enteringSlowMo = false;
	public static bool exitingSlowMo = false;
	public const float timeToFast = .4f;
	public const float timeToSlow = .25f;
	public const float slowScale = .1f;
	public const float distanceBeforeWallToStartSlowMo = 70f;
	private float timeCounter;
	public PlayerController Player;
	public Text PauseButtonText;
	public LevelController LevelController;
    

	public Button howTo;

	void Awake() {
		EnterSlowMotionPosition = 999999999f;
        ExitSlowMotion();
	}

	// Update is called once per frame
	void Update () {
		if(!IsPaused) {
			howTo.gameObject.SetActive(IsPaused);
			if (Player.transform.position.x > EnterSlowMotionPosition) {
				EnterSlowMotion();
				EnterSlowMotionPosition = 999999999f;
			} else if(WallDestroyed) {
				WallDestroyed = false;
				ExitSlowMotion();
			}
			if(enteringSlowMo) {
				/*timeCounter += Time.deltaTime;
				if(timeCounter >= timeToSlow) {
					enteringSlowMo = false;
					Time.timeScale = slowScale;
				} else {
					Time.timeScale = Mathf.Lerp(1f, slowScale, timeCounter/timeToSlow);
				}*/
				float extra = (1f - PercentToWall()) * (1f - slowScale);
				Time.timeScale = slowScale + extra;
			}

			if(exitingSlowMo) {
				timeCounter += Time.deltaTime;
				if(timeCounter >= timeToFast) {
					exitingSlowMo = false;
					Time.timeScale = 1f;
				} else {
					Time.timeScale = Mathf.Lerp(slowScale, 1f, timeCounter/timeToFast);
				}
			}
		} else {
			howTo.gameObject.SetActive(IsPaused);
			Time.timeScale = 0f;
		}
	}

	float PercentToWall() {
		float playerX = Player.transform.position.x;
		return (playerX - (LevelController.CurrentWallPosition - distanceBeforeWallToStartSlowMo)) / (distanceBeforeWallToStartSlowMo);
	}

	public void EnterSlowMotion() {
		if(!enteringSlowMo) {
			timeCounter = 0f;
			enteringSlowMo = true;
		}
	}

	public void ExitSlowMotion() {
		if(!exitingSlowMo) {
			enteringSlowMo = false;
			timeCounter = 0f;
			exitingSlowMo = true;
		}
	}

	public static void Pause(bool pause) {
		IsPaused = pause;
		
		Time.timeScale = IsPaused ? 0f : 1;
	}
	

	// for Buttons
	public void TogglePause() {
		Pause (!IsPaused);
		PauseButtonText.text = IsPaused ? "Play" : "Pause";
	}
}
