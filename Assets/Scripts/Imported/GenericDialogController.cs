using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GenericDialogController : DialogController {
	public Text titleText;
	public Text messageText;
	
	public Text option1Text;
	public Text option2Text;
	public Text option3Text;

	public void Show(Action<int> callback, string title, string message, string option1, string option2 = null, string option3 = null) {
		base.Show(callback);
		titleText.text = title;
		messageText.text = message;
		
		if(option1Text != null && option1 != "") {
			option1Text.text = option1;
		}
		
		if(option2Text != null && option2 != null) {
			option2Text.text = option2;
		}
		
		if(option3Text != null && option3 != null) {
			option3Text.text = option3;
		}
	}

}