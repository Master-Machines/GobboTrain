﻿using UnityEngine;
using System.Collections;

public class ObstacleCreator : MonoBehaviour {
	public const float SpawnDistance = 120f;

	public LevelController LevelController;
	public PlayerController PlayerController;
	public GameController GameController;

	public float RowDistance {get;set;}
	public float CurrentRowPosition {get;set;}
	private float RowDistanceModifier {get;set;}

	public GameObject[] ObstaclePrefabs;
	public GameObject PowerBoostPrefab;
	public GameObject SpeedBoostPrefab;
	public GameObject SpeedBoostShroomPrefab;
	public GameObject GoblinPrefab;
	public GameObject RunnerPrefab;

	public static float ChanceOfGoblin = 0f;
	public static float ChanceOfSpeed = 0f;
	public static float ChanceOfRunner = 0f;

	public int MinimumNumberOfObstacles {get;set;}
	public int MaximumNumberOfObstacles {get;set;}
	public int MinimumObstacleIndex {get; set;}
	public float MaximumObstacleIndex {get; set;}

	void Start () {
		MinimumNumberOfObstacles = 0;
		MaximumNumberOfObstacles = 1;
		RowDistance = 40f;
		CurrentRowPosition = 0f;
		CurrentRowPosition = 160f;
		RowDistanceModifier = 1f;
		MaximumObstacleIndex = 1f;
		IncreaseDifficulty();
	}

	public void IncreaseDifficulty() {
		MaximumNumberOfObstacles ++;
		if(MaximumNumberOfObstacles > 3) {
			MaximumNumberOfObstacles = 3;
		}
	
		MaximumObstacleIndex += 1f;

		RowDistanceModifier *= .75f;
	}

	void Update () {
		if(PlayerController.transform.position.x >= CurrentRowPosition - SpawnDistance)
			CreateNextRow();
		RowDistance = 10f + 25f * RowDistanceModifier;
	}

	void CreateNextRow() {
		int[] hasObstacles = new int[]{0, 0, 0};
		int numberOfObstacles = Random.Range (MinimumNumberOfObstacles, MaximumNumberOfObstacles);
		for(int i = 0; i < numberOfObstacles; i++) {
			hasObstacles[Random.Range(0, 3)] = 1;
		}


		hasObstacles[GetAvailablePosition(hasObstacles)] = 2;

		for(int i = 0; i < 3; i++) {
			if(hasObstacles[i] == 1)
				CreateObstacle(i);
			else if(hasObstacles[i] == 2)
				CreatePowerup(i);
		}

		ChanceOfRunner += .005f;
		if(Random.Range (0f, 1f) < ChanceOfRunner) {
			GameObject runner = (GameObject)Instantiate(RunnerPrefab, new Vector3(CurrentRowPosition, -3.195f, Random.Range (PlayerController.RightLanePosition, PlayerController.LeftLanePosition)), Quaternion.Euler(new Vector3(0f, 90f, 0f)));
			ChanceOfRunner = 0.01f;
			runner.transform.SetParent(transform);
		}

		CurrentRowPosition += RowDistance * Random.Range (.9f, 1.1f);
	}

	int GetAvailablePosition(int[] positions) {
		int selectedIndex = Random.Range(0, 3);
		while(positions[selectedIndex] != 0) {
			selectedIndex = Random.Range(0, 3);
		}
		return selectedIndex;
	}

	void CreateObstacle(int lane) {
		if(!LevelController.IsPositionNearWall((int)CurrentRowPosition, 20)) {
			float position = GetPositionFromLane(lane);
			int maxInt = (int)MaximumObstacleIndex;
			int selectedIndex = Random.Range (MinimumObstacleIndex, maxInt);
			if(selectedIndex >= ObstaclePrefabs.Length)
				selectedIndex = ObstaclePrefabs.Length - 1;
			
			GameObject createdObj = (GameObject)Instantiate(ObstaclePrefabs[selectedIndex], new Vector3(CurrentRowPosition + Random.Range (-3.5f, 3.5f), transform.position.y, position), Quaternion.Euler(0, 0, 0));
			createdObj.transform.SetParent(this.transform);
			createdObj.GetComponent<Obstacle>().Player = PlayerController;
		}
	}

	void CreatePowerup(int lane) {
		float position = GetPositionFromLane(lane);
		Vector3 spawnPosition = new Vector3(CurrentRowPosition + Random.Range (-3.5f, 3.5f), transform.position.y, position);
		ChanceOfSpeed += .02f;
		ChanceOfGoblin += .02f;
		GameObject createdObj = null;

		if(Random.Range(0f, 1f) < ChanceOfGoblin) {
			createdObj = (GameObject)Instantiate(GoblinPrefab, spawnPosition, Quaternion.identity);
			ChanceOfGoblin = .03f;
		} else if(Random.Range(0f, 1f) < ChanceOfSpeed) {
			int i = Random.Range(0, 100);
			//if (i < 50) createdObj = (GameObject)Instantiate(SpeedBoostPrefab, spawnPosition, Quaternion.identity);
			createdObj = (GameObject)Instantiate(SpeedBoostShroomPrefab, spawnPosition, Quaternion.identity);
			ChanceOfSpeed = .03f;
		}
		if(createdObj != null) {
			createdObj.transform.SetParent(this.transform);
			createdObj.transform.Translate(-Vector3.up * 2.5f);
		}
	}

	float GetPositionFromLane(int lane) {
		float position = PlayerController.MiddleLanePosition;
		if(lane == 0)
			position = PlayerController.LeftLanePosition;
		else if(lane == 2)
			position = PlayerController.RightLanePosition;
		position += Random.Range (-1.25f, 1.25f);
		return position;
	}
}
