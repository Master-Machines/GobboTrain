using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class StartGameDialog : DialogController {

	public Text Total;
    float hudHeight;
    float hudWidth;
    GameObject hudParent;
	void Start() {
        hudParent = GameObject.Find("HUD");
        RectTransform hudRt = (RectTransform)hudParent.transform;
        hudHeight = hudRt.rect.height;
        hudWidth = hudRt.rect.width;
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

	public void GoToShop() {
		Application.LoadLevel("Store");
	}
}
