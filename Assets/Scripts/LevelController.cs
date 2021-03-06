﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {
	public GameObject TunnelPrefab;
	public PlayerController Player;
	public GameController GameController;

	public GameObject[] WallPrefabs;

	private List<Tunnel> Tunnels = new List<Tunnel>();
	private Vector3 CurrentPosition;
	private float creationPosition = 0f;

	public const float TunnelLength = 30f;

	public int MaxObstacles;
	public int MinObstacles;
	public int MaxObstacleDifficulty;

	public int NextWallPosition;
	public int CurrentWallPosition;
	public const int WallWarningDistance = 350;
	private int wallCounter = 0;
	public int WallHealth;
	private int WallHealthIncreaseAmount;
	private int WallHealthIncreaseDecreaseAmount;
	private int MinWallHealthIncreaseAmount;
	public bool WallActive = false;
	public ObstacleCreator ObstacleCreator;

	public Image howToScreen;
	public Button howToGo, howToBack;

	// Use this for initialization
	void Start () {
		NextWallPosition += 1400;
		WallHealth = 70;
		MinObstacles = 0;
		MaxObstacles = 2;
		MaxObstacleDifficulty = 1;
		MinWallHealthIncreaseAmount = 5;
		WallHealthIncreaseDecreaseAmount = 3;
		MinWallHealthIncreaseAmount = 19;
		CurrentPosition = transform.position;
		for(int i = 0; i < 5; i++) {
			if(i == 0)
				CreateNextPiece(false, true);
			else
				CreateNextPiece(false, false);
		}
		creationPosition = 15f;
	}

	// Update is called once per frame
	void Update () {
		if(Player.transform.position.x > creationPosition) {
			CreateNextPiece(true, false);
		}
		CheckForWalls();
	}

	public void HowToPlayButton()
	{
		howToScreen.gameObject.SetActive(true);
		howToGo.gameObject.SetActive(false);
		howToBack.gameObject.SetActive(true);
	}

	public void HowToBack()
	{
		howToScreen.gameObject.SetActive(false);
		howToGo.gameObject.SetActive(true);
		howToBack.gameObject.SetActive(false);
	}

	void CheckForWalls() {
		if(!WallActive){
			if(Player.transform.position.x > NextWallPosition - WallWarningDistance) {
				wallCounter ++;
				GameController.DisplayWallWarning();
				WallActive = true;
				GameObject createdWall = (GameObject)Instantiate(WallPrefabs[0], new Vector3(NextWallPosition, 0f, 0f), Quaternion.identity);
				Obstacle ob = createdWall.GetComponent<Obstacle>();
				TextMesh text = createdWall.GetComponentInChildren<TextMesh>();
				text.text = "wall " + wallCounter.ToString();
				ob.RequiredMomentum = WallHealth;
				CurrentWallPosition = NextWallPosition;
				TimeController.EnterSlowMotionPosition = NextWallPosition - TimeController.distanceBeforeWallToStartSlowMo;
			}
		}
	}

	public void WallDestroyed() {
		WallHealth += WallHealthIncreaseAmount;
		WallHealthIncreaseAmount -= WallHealthIncreaseDecreaseAmount;
		if(WallHealthIncreaseAmount < MinWallHealthIncreaseAmount)
			MinWallHealthIncreaseAmount = MinWallHealthIncreaseAmount;

		TimeController.WallDestroyed = true;
		NextWallPosition += 1400 - (40 * wallCounter);
		WallActive = false;
		ObstacleCreator.IncreaseDifficulty();
	}

	void CreateNextPiece(bool updateCreationPosition, bool first) {
		Vector3 position = CurrentPosition;
		GameObject createdObj = (GameObject)Instantiate(TunnelPrefab, position, Quaternion.LookRotation(new Vector3(1f, 0, 0)));
		Tunnel tunnel = createdObj.GetComponent<Tunnel>();
		Tunnels.Add(tunnel);
		int numObstacles = first ? 0 : DetermineNumberOfObstacles();
		tunnel.Setup(this, numObstacles, MaxObstacleDifficulty, this);

		CurrentPosition.x += TunnelLength;
		if(updateCreationPosition)
			creationPosition += TunnelLength;
	}

	int DetermineNumberOfObstacles() {
		return Random.Range(MinObstacles, MaxObstacles);
	}

	public void RemoveTunnel(Tunnel tunnel) {
		Tunnels.Remove(tunnel);
	}

	public bool IsPositionNearWall(int position, int threshold) {
		return (position > NextWallPosition - threshold) && (position < NextWallPosition + threshold);
	}
}
