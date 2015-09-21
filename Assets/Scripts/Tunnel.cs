using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tunnel : MonoBehaviour {
	private LevelController Controller;
	private int counter = 0;
	
	public GameObject PowerBoostPrefab;
	public GameObject SpeedBoostPrefab;
	public GameObject GoblinPrefab;
	public GameObject[] LaneDividerPrefabs;
	public GameObject[] EnvironmentPrefabs;
	private List<Obstacle> createdObstacles = new List<Obstacle>();
	public static float ChanceOfGoblin = 0f;
	public static float ChanceOfSpeed = 0f;
	public static float ChanceOfPowerBoost = 0f;
	public LevelController LevelController {get; set;}

	public void Setup(LevelController controller, int obstacleCount, int maxObstacleDifficulty, LevelController levelController) {
		Controller = controller;
		LevelController = levelController;
		// CreateLaneObstacles(obstacleCount, maxObstacleDifficulty);
		CreateEnvironment();
		ChanceOfGoblin += Random.Range (.03f, .07f);
		ChanceOfSpeed += Random.Range (.05f, .06f);
		ChanceOfPowerBoost += Random.Range(.07f, .08f);

		if(Random.Range (0f, 1f) < ChanceOfGoblin) {
			CreateObstacle(true, 0, GoblinPrefab);
			ChanceOfGoblin = 0f;
		}

		if(Random.Range (0f, 1f) < ChanceOfSpeed) {
			CreateObstacle(true, 0, SpeedBoostPrefab);
			ChanceOfSpeed = .1f;
		}

		if(Random.Range (0f, 1f) < ChanceOfPowerBoost) {
			//CreateObstacle(true, 0, PowerBoostPrefab);
			ChanceOfPowerBoost = .12f;
		}


		CreateLaneDividers(maxObstacleDifficulty - 1);
	}

	void CreateLaneObstacles(int obstacleCount, int maxObstacleDifficulty) {
		for(int i = 0; i < obstacleCount; i++) {
			if(!CreateObstacle(false, maxObstacleDifficulty)) {
				// Failed to make an obstacle, so just break out of the loop
				i = obstacleCount;
			}
		}
	}

	void CreateLaneDividers(int max) {
		int maxDivider = Random.Range(0, 5);
		for(int i = 0; i < maxDivider; i++) {
			CreateLaneDividerObstacle(max);
		}
	}

	void CreateEnvironment() {
		int numEnvironmentObjects = Random.Range(0, 3);
		for(int i = 0; i < numEnvironmentObjects; i++) {
			CreateEnvironmentObject();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(counter++ > 10){ 
			counter = 0;
			if(transform.position.x < Controller.Player.transform.position.x - LevelController.TunnelLength) {
				Controller.RemoveTunnel(this);
				Destroy(gameObject);
			}
		}
	}

	// TODO: This code sucks
	void CreateEnvironmentObject() {
		Vector3 spawnPosition = GetRandomEdgePosition();
		int selectedIndex = Random.Range(0, EnvironmentPrefabs.Length);
		if(selectedIndex == 1 && Controller.Player.transform.position.x < Random.Range(0, 5000))
			selectedIndex = 0;
		GameObject obj = (GameObject)Instantiate(EnvironmentPrefabs[selectedIndex], spawnPosition, Quaternion.identity);
		if(selectedIndex == 0) {
			obj.transform.Rotate(new Vector3(0f, Random.Range(0f, 360f)));
		} else {
			// Goblin home
			if(spawnPosition.z < 0) {
				obj.transform.Rotate(new Vector3(0f, 180f));
				obj.transform.Translate(new Vector3(0f, 0f, -.75f), Space.World);
			} else {
				obj.transform.Translate(new Vector3(0f, 0f, .75f), Space.World);
			}
		}
		float scaleModifier = Random.Range(.8f, 1.5f);
		Vector3 localScale = obj.transform.localScale;
		localScale.x *= scaleModifier;
		localScale.z *= scaleModifier;
		obj.transform.localScale = localScale;

		obj.transform.parent = transform;
	}

	void CreateLaneDividerObstacle(int max) {
		Vector3 SelectedPosition = GetRandomLaneDividerSpawnPosition();
		int failSafe = 10;
		while(PositionIsNearOtherObstacles(SelectedPosition, false) && failSafe-- > 0) {
			SelectedPosition = GetRandomLaneDividerSpawnPosition();
		}
		
		if(failSafe > 0) {
			if(max > LaneDividerPrefabs.Length)
				max = LaneDividerPrefabs.Length;
			GameObject obj = (GameObject)Instantiate(LaneDividerPrefabs[Random.Range(0, max)], SelectedPosition, Quaternion.identity);
			obj.transform.Rotate(new Vector3(0f, Random.Range(0f, 360f)));
			Obstacle obstacle = obj.GetComponent<Obstacle>();
			createdObstacles.Add(obstacle);
			obj.transform.parent = transform;
		}
	}

	public bool CreateObstacle(bool isPowerup, int maxObstacleDifficulty, GameObject powerupPrefab = null) {
		Vector3 SelectedPosition = GetRandomObstacleSpawnPosition();
		int failSafe = 20;
		while(PositionIsNearOtherObstacles(SelectedPosition, !isPowerup) && failSafe-- > 0) {
			SelectedPosition = GetRandomObstacleSpawnPosition();
		}

		if(failSafe > 0) {
			if(isPowerup) {
				GameObject obj = (GameObject)Instantiate(powerupPrefab, SelectedPosition, Quaternion.identity);
				obj.transform.parent = transform;
			}
			return true;
		}
		return false;
	}

	bool PositionIsNearOtherObstacles(Vector3 position, bool useSize) {
		foreach(Obstacle ob in createdObstacles) {
			float size = 1f;
			if(useSize)
				size = ob.Size;
			if(Vector3.Distance(position, ob.transform.position) < size * 1.6f) {
				return true;
			}
		}
		if(PlayerController.RoughlyEqual(position.x, LevelController.NextWallPosition, 4f)) {
			return true;
		}
		return false;
	}

	Vector3 GetRandomEdgePosition() {
		float zPosition = (Random.Range(0, 2) == 0 ? -8.75f : 8.75f);
		zPosition += Random.Range(-.5f, .5f);
		float xPosition = Random.Range(-15f, 15f);
		return new Vector3(transform.position.x + xPosition,transform.position.y + 0f,transform.position.z + zPosition);
	}

	Vector3 GetRandomLaneDividerSpawnPosition() {
		float zPosition = (PlayerController.RightLanePosition + PlayerController.MiddleLanePosition) / 2f;
		if(Random.Range (0, 2) == 0) {
			zPosition = (PlayerController.LeftLanePosition + PlayerController.MiddleLanePosition) / 2f;
		}
		float xPosition = Random.Range(-15f, 15f);
		return new Vector3(transform.position.x + xPosition, transform.position.y - 3f, transform.position.z + zPosition);
	}

	Vector3 GetRandomObstacleSpawnPosition() {
		PlayerController.Lane lane = (PlayerController.Lane)Random.Range(-1, 2);
		float zPosition = PlayerController.GetPositionFromLane(lane);
		zPosition += Random.Range(-.5f, .5f);

		float xPosition = Random.Range (-15f, 15f);

		return new Vector3(transform.position.x + xPosition, transform.position.y - 3f, transform.position.z + zPosition);
	}
}
