using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerController : MonoBehaviour {

	public PlayerController Player;

	public bool PowerupAvailable{get;set;}
	public GameObject PowerLight;
	public Material GobboMat;
	public Color PowerColor;

	public enum PowerType {
		Shockwave,
		SpeedBoost
	}

	public PowerType CurrentPower;

	// Use this for initialization
	void Start () {
		EnablePowerup(false);
		CurrentPower = PowerType.SpeedBoost;
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
		PowerupAvailable = enabled;
		PowerLight.SetActive(enabled);
		if(PowerupAvailable) {
			GobboMat.SetColor("_EmissionColor", PowerColor);
		} else {
			GobboMat.SetColor("_EmissionColor", Color.black);
		}
	}

	IEnumerator Cooldown() {
		yield return new WaitForSeconds(8f);
		EnablePowerup(true);
	}

	void DoPowerup() {
		if(CurrentPower == PowerType.Shockwave) {
			Player.Bomb();
		} else if(CurrentPower == PowerType.SpeedBoost) {
			Player.SpeedPower();
		}
	}
}
