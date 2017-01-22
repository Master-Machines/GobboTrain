using UnityEngine;
using System.Collections;

public class MushScript : MonoBehaviour {
	Animation animation = new Animation();
	// Use this for initialization
	void Start () {
		animation.Play("MushAnim");
	}
	
	// Update is called once per frame
	void Update () {
	}
}
