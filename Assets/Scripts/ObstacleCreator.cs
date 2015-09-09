using UnityEngine;
using System.Collections;

public class ObstacleCreator : MonoBehaviour {
	public const float SpawnDistance = 155f;

	public LevelController LevelController;
	public PlayerController PlayerController;
	public GameController GameController;

	public float RowDistance {get;set;}
	public float CurrentRowPosition {get;set;}

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
	}

	public IEnumerator IncreaseDifficulty() {
		yield return new WaitForSeconds(5f);
		if(Random.Range (0f, 1f) < .25f) {
			MaximumNumberOfObstacles ++;
			if(MaximumNumberOfObstacles > 3) {
				MaximumNumberOfObstacles = 3;
				MinimumNumberOfObstacles ++;
				if(MinimumNumberOfObstacles > 1)
					MinimumNumberOfObstacles = 1;
			}
		}
		
		if(Random.Range(0f, 1f) < .5f) {
			MaximumObstacleIndex ++;
		}

		RowDistance *= Random.Range(.93f, .97f);
		if(RowDistance < 15f)
			RowDistance = 15f;

		StartCoroutine(IncreaseDifficulty());
	}

	void Update () {
		if(PlayerController.transform.position.x >= CurrentRowPosition - SpawnDistance)
			CreateNextRow();
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


		CurrentRowPosition += RowDistance * Random.Range (.75f, 1.25f);
	}

	void CreateObstacle(int lane) {
		float position = GetPositionFromLane(lane);

		int selectedIndex = Random.Range (MinimumObstacleIndex, MaximumObstacleIndex);
		if(selectedIndex >= ObstaclePrefabs.Length)
			selectedIndex = ObstaclePrefabs.Length - 1;

		GameObject createdObj = (GameObject)Instantiate(ObstaclePrefabs[selectedIndex], new Vector3(CurrentRowPosition, transform.position.y, position), Quaternion.identity);
		createdObj.transform.SetParent(this.transform);
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
