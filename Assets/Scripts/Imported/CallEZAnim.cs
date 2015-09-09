using UnityEngine;
using System.Collections;
using MaterialUI;

public class CallEZAnim : MonoBehaviour {
	public string functionName;
	// Use this for initialization
	IEnumerator Start () {
		yield return 1;
		GetComponent<EZAnim>().AnimateByName(functionName);
	}
}
