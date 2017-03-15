using UnityEngine;
using System.Collections;
using System;

/*public class DialogGenerator : MonoBehaviour{
	private static GameObject OneOptionPrefab;
	private static GameObject TwoOptionPrefab;
	private static GameObject ThreeOptionPrefab;
	
	public static void Init(GameObject oneOption, GameObject twoOption, GameObject threeOption) {
		OneOptionPrefab = oneOption;
		TwoOptionPrefab = twoOption;
		ThreeOptionPrefab = threeOption;
	}
	
	public static void CreateDialog(Action<int> callback, string title, string message, string option1, string option2 = null, string option3 = null) {
		GameObject selectedPrefab = OneOptionPrefab;
		if(option2 != null) {
			selectedPrefab = TwoOptionPrefab;
		}
		
		if(option3 != null) {
			selectedPrefab = ThreeOptionPrefab;
		}
		
		GameObject createdDialog = (GameObject)Instantiate(selectedPrefab);
		createdDialog.transform.SetParent(GameObject.Find("HUD").transform);
		createdDialog.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 500);
		createdDialog.GetComponent<GenericDialogController>().Show(callback, title, message, option1, option2, option3);
	}

	public static void CreateCustomDialog(string DialogName, Vector2 Position, Action<GameObject> creationCallback, Action<int> resultCallback) {
		GameObject obj = (GameObject)Instantiate(Resources.Load("Dialogs/" + DialogName));
		obj.transform.SetParent(GameObject.Find ("HUD").transform);
		obj.GetComponent<RectTransform>().anchoredPosition = Position;
		if(creationCallback != null)
			creationCallback(obj);
		obj.GetComponent<DialogController>().Show(resultCallback);
	}
}*/
