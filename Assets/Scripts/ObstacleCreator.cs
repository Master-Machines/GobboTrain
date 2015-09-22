using UnityEngine;
using System.Collections;

public class ObstacleCreator : MonoBehaviour {
	public const float SpawnDistance = 105f;

	public LevelController LevelController;
	public PlayerController PlayerController;
	public GameController GameController;

	public float RowDistance {get;set;}
	public float CurrentRowPosition {get;set;}
	private float RowDistanceModifier {get;set;}

	public GameObject[] ObstaclePrefabs;
	public GameObject PowerBoostPrefab;
	public GameObject SpeedBoostPrefab;
	public GameObject GoblinPrefab;

	public int MinimumNumberOfObstacles {get;set;}
	public int MaximumNumberOfObstacles {get;set;}
	public int MinimumObstacleIndex {get; set;}
	public int MaximumObstacleIndex {get; set;}

	void Start () {
		MinimumNumberOfObstacles = 0;
		MaximumNumberOfObstacles = 2;
		RowDistance = 40f;
		CurrentRowPosition = 0f;
		CurrentRowPosition = 160f;
		RowDistanceModifier = 1f;
		IncreaseDifficulty();
	}

	public void IncreaseDifficulty() {
		MaximumNumberOfObstacles ++;
		if(MaximumNumberOfObstacles > 3) {
			MaximumNumberOfObstacles = 3;
		}
	
		MaximumObstacleIndex ++;

		RowDistanceModifier *= .8f;
	}

	void Update () {
		if(PlayerController.transform.position.x >= CurrentRowPosition - SpawnDistance)
			CreateNextRow();
		RowDistance = 10f + PlayerController.GetSpeed() * .4f * (.1f + RowDistanceModifier);
	}

	void CreateNextRow() {
		int[] hasObstacles = new int[]{0, 0, 0};
		int numberOfObstacles = Random.Range (MinimumNumberOfObstacles, MaximumNumberOfObstacles);
		for(int i = 0; i < numberOfObstacles; i++) {
			hasObstacles[Random.Range(0, 3)] = 1;
		}


		for(int i = 0; i < 3; i++) {
			if(hasObstacles[i] == 1)
				CreateObstacle(i);
			else if(hasObstacles[i] == 2)
				CreatePowerup(i);
		}


		CurrentRowPosition += RowDistance * Random.Range (.9f, 1.1f);
	}

	void CreateObstacle(int lane) {
		if(!LevelController.IsPositionNearWall((int)CurrentRowPosition, 80)) {
			float position = GetPositionFromLane(lane);
			
			int selectedIndex = Random.Range (MinimumObstacleIndex, MaximumObstacleIndex);
			if(selectedIndex >= ObstaclePrefabs.Length)
				selectedIndex = ObstaclePrefabs.Length - 1;
			
			GameObject createdObj = (GameObject)Instantiate(ObstaclePrefabs[selectedIndex], new Vector3(CurrentRowPosition + Random.Range (-3f, 3f), transform.position.y, position), Quaternion.identity);
			createdObj.transform.SetParent(this.transform);
		}
	}

	void CreatePowerup(int lane) {
		float position = GetPositionFromLane(lane);
	}

	float GetPositionFromLane(int lane) {
		float position = PlayerController.MiddleLanePosition;
		if(lane == 0)
			position = PlayerController.LeftLanePosition;
		else if(lane == 2)
			position = PlayerController.RightLanePosition;
		position += Random.Range (-1f, 1f);
		return position;
	}
}
