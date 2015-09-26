using UnityEngine;
using System.Collections;

public class CameraConroller : MonoBehaviour {
	public PlayerController Player;
	public TimeController TimeController;
	public Camera camera;
	private const float MinCameraFOV = 40f;
	private const float BonusCameraFOV = 20f;
	private const float CameraFOVAdjustmentSpeed = 25f;
	private float maxDelta;

	// Use this for initialization
	void Start () {
		camera.fieldOfView = MinCameraFOV;
		maxDelta = 1f - TimeController.slowScale;
	}
	
	// Update is called once per frame
	void Update () {
		if(!TimeController.IsPaused) {
			if(Time.timeScale < 1f) {
				UpdateCameraSlowMo();
			} else {
				UpdateCameraNormal();
			}
		}
	}

	void UpdateCameraSlowMo() {
		float delta = Time.timeScale - TimeController.slowScale;
		float SlowPercent = 1f - ((maxDelta - delta) / maxDelta);
		Debug.Log (SlowPercent);
		camera.fieldOfView = MinCameraFOV + (SlowPercent * BonusCameraFOV);
	}

	void UpdateCameraNormal() {
		float TargetCameraFOV = MinCameraFOV + ((Player.GetSpeed() / Player.MaxSpeed)) * BonusCameraFOV;
		if(PlayerController.RoughlyEqual(camera.fieldOfView, TargetCameraFOV, .5f)) {
			camera.fieldOfView = TargetCameraFOV;
		} else if(camera.fieldOfView < TargetCameraFOV) {
			camera.fieldOfView += CameraFOVAdjustmentSpeed * Time.deltaTime;
		} else {
			camera.fieldOfView -= CameraFOVAdjustmentSpeed * Time.deltaTime;
		}
	}
}
