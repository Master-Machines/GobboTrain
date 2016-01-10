using UnityEngine;
using System.Collections;

public class RunningGobbo : MonoBehaviour {
	private float Speed;
	public GameObject DeathParticles;
	private float RunningAngle;
	// Use this for initialization
	void Start () {
		RunningAngle = Random.Range (75, 105f);
		Speed = Random.Range (20f, 30f);
		SetRotation();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);
		CheckForSides();
	}

	void CheckForSides() {
		if(transform.position.z > PlayerController.LeftLanePosition + 1f && RunningAngle < 90f) {
			RunningAngle = Random.Range (90f, 120f);
			SetRotation();
		} else if(transform.position.z < PlayerController.RightLanePosition - 1f && RunningAngle > 90f) {
			RunningAngle = Random.Range (60f, 90f);
			SetRotation();
		}
	}

	void SetRotation() {
		transform.rotation = Quaternion.Euler(new Vector3(0f, RunningAngle, 0f));
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Obstacle")) {
			Obstacle obstacle = other.gameObject.GetComponent<Obstacle>();
			obstacle.Break();
		}
	}

	public void Death(Transform parent) {
		GameObject death = (GameObject)Instantiate(DeathParticles, transform.position, Quaternion.Euler(new Vector3(0f, 270f, 0f)));
		death.transform.SetParent(parent);
		Destroy(gameObject);
	}
}
