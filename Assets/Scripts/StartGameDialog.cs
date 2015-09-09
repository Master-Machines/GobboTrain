using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class StartGameDialog : DialogController {

	public Toggle TiltToggle;
	public Text Total;

	void Start() {
		TiltToggle.isOn = Global.Instance.tiltEnabled;
		Total.text = Global.Instance.currency.ToString() + "c";
	}

	public void TiltControls(bool tiltEnabled) {
		Global.Instance.tiltEnabled = tiltEnabled;
		if(tiltEnabled) {
			Screen.orientation = ScreenOrientation.Landscape;
		} else {
			Screen.orientation = ScreenOrientation.AutoRotation;
		}
	}
}
