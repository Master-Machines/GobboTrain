using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class DialogController : MonoBehaviour {
	

	
	private Action<int> callbackFunction;
	
	public void Show(Action<int> callback) {
		callbackFunction = callback;
	}
	
	public void PlayerClickedButton(int response) {
		callbackFunction(response);
	}
}
