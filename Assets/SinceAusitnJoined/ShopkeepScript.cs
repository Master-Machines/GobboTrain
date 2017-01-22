using UnityEngine;
using System.Collections;

public class ShopkeepScript : MonoBehaviour {

	int x;
	float time;
	// Use this for initialization
	void Start () {
		//gameObject.GetComponent<Animation>().Play("Idle");
		Time.timeScale = 1.0f;
		Debug.Log(Time.timeScale);
	}
	
	// Update is called once per frame
	void Update () {
		x = Random.Range(0, 100);
		time += Time.deltaTime;

		if(x == 75 && time > 1)
		{
			gameObject.GetComponent<Animation>().PlayQueued("Talking");
			time = 0;
		}
		else if(x != 75 && time > 1)
		{
			gameObject.GetComponent<Animation>().PlayQueued("Idle");
			time = 0;
		}
	}
}
