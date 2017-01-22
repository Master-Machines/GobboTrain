using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerController : MonoBehaviour {

	public PlayerController Player;

	public bool PowerupAvailable;
	public GameObject PowerLight;
	public Material GobboMat;
	public Color PowerColor;

	public enum PowerType {
		Shockwave,
		SpeedBoost,
		AllGold,
		NULL
	}


	public PowerType CurrentPower = PowerType.NULL;

	// Use this for initialization
	void Start () {

		EnablePowerup(false);
		PowerupAvailable = false;
		PowerLight.SetActive(false);
		if (Global.Instance.SelectedPowerup == -1) CurrentPower = PowerType.NULL;
		else CurrentPower = (PowerType) Global.Instance.SelectedPowerup;
		print(Global.Instance.SelectedPowerup);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool AttemptPower() {
		if(PowerupAvailable) {
			EnablePowerup(false);
			DoPowerup();
			return true;
		}

		return false;
	}

	public void EnablePowerup(bool enabled) {
		if (CurrentPower == PowerType.AllGold || CurrentPower == PowerType.Shockwave || CurrentPower == PowerType.SpeedBoost)
		{
			print("Inside if");
			PowerupAvailable = enabled;
			PowerLight.SetActive(enabled);
			if (PowerupAvailable && Global.Instance.SelectedPowerup != 0)
			{
				GobboMat.SetColor("_EmissionColor", PowerColor);
			}
			else
			{
				GobboMat.SetColor("_EmissionColor", new Color(0.1f, 0.1f, 0.1f));
			}
		}
	}

	IEnumerator Cooldown() {
		yield return new WaitForSeconds(8f);
		if(PowerupAvailable) EnablePowerup(true);
	}

	void DoPowerup() {
		if(CurrentPower == PowerType.Shockwave) {
			Player.Bomb();
		} else if(CurrentPower == PowerType.SpeedBoost) {
			Player.SpeedPower();
		} else if (CurrentPower == PowerType.AllGold){
			Player.AllGold();
		}
	}
}
