using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private float Speed = 0f;
	private float BonusSpeed = 0f;

	public AudioClip gobboSmashSmall, gobboSmashMed, gobboSmash, gobboOnGobbo, footHit;
	public AudioSource soundSource;

	public const float LeftLanePosition = 5f;
	public const float MiddleLanePosition = 0f;
	public const float RightLanePosition = -5f;
	private const float ExtraLaneSwitchTime = 0f;
	private const float MinLaneSwitchTime = .15f;
	public float ModerateSpeed = 35f;
	public float MaxSpeed = 55f;
	public const float GodSpeed = 85f;

	private const float SpeedBoostBonus = 15f;
	private const float SpeedBoostTime = 3f;
	private const float BombDistance = 50f;
	private const float LeftTilt = -.185f;
	private const float RightTilt = .185f;

	public GameObject Model;
	public GameObject GoblinDeathParticles;

	public int MaxGoblins = 5;
	public int CurrentGoblins = 0;
	public float GoblinPowerBonus = .5f;

	public const float DistanceBetweenPoints = 50f;
	public float LastDistanceWherePointsWereGiven = 0f;

	public GameObject[] ExtraGobbos;

	public enum Lane {
		Left = -1,
		Middle = 0,
		Right = 1
	}

	public Lane CurrentLane;
	public bool SwitchingLanes = false;
	private bool InputPressed = false;
	private float LaneSwitchTime = .4f;
	public GameController GameController;
	private float delayCounter = 0f;
	private float postPowerCounter = 0f;

	private float LaneSwitchCounter = 0f;
	private Lane TargetLane;
	public int MomentumBonuses = 0;
	public LevelController LevelController;
	public GameObject SpeedPickupExplosion;
	public GameObject MultiplierPickupExplosion;
	private bool Dead = false;
	public int Bombs = 3;
	public float ImpactSpeed;

	public Material[] ObstacleMaterials;
	public float[] ObstacleMomentums;

	public Animation[] runAnims;

	public GameObject RagdollPrefab;
	public SwipeDetector SwipeDetector;
	public PowerController PowerController;

	// Use this for initialization
	void Start () {
		Speed = 10f;
		CurrentLane = Lane.Middle;
		SwipeDetector.SwipeEvents.Add(SwipeHappened);
	}
	
	// Update is called once per frame
	void Update () {
		if(!Dead && !TimeController.IsPaused) {
			UpdatePosition();
			CheckInput();
			UpdateSpeed(true);
			//UpdateObstacleMaterials();
			if(Input.GetButtonDown("Jump") && Bombs-- > 0) {
				PowerController.AttemptPower();
			}
		}
	}

	void UpdateSpeed(bool on) {
		if(Speed < ModerateSpeed) { // HERE IS WHERE RUNNER CLASS IS USED
			Speed += (Time.deltaTime * 10f + (Global.Instance.Specialty3Level * 0.2f));
		} else if(Speed < MaxSpeed) {
			Speed += Time.deltaTime * 5f + (Global.Instance.Specialty3Level * 0.2f);
		} else if (Speed < GodSpeed) {
			Speed += Time.deltaTime * 1f + (Global.Instance.Specialty3Level * 0.2f);
		} else {
			Speed *= .99f;
		}

		BonusSpeed *= (1f - (.5f * Time.deltaTime));

		float extraTime = (1 - ((Speed + BonusSpeed) / MaxSpeed)) * ExtraLaneSwitchTime;
		if(extraTime < 0)
			extraTime = 0;
		LaneSwitchTime = MinLaneSwitchTime + extraTime;
		float animSpeed = 0f;
		if(on) {
			animSpeed = .75f + Speed * .015f;
		}
		foreach(Animation anim in runAnims) {
			anim["Run"].speed = animSpeed;
		}
	}

	void UpdatePosition() {
		transform.Translate(new Vector3(Speed + BonusSpeed, 0f, 0f) * Time.deltaTime);
		if(SwitchingLanes) {
			LaneSwitchCounter += Time.deltaTime;
			if(LaneSwitchCounter >= LaneSwitchTime) {
				Vector3 currentPosition = transform.position;
				currentPosition.z = GetPositionFromLane(TargetLane);
				transform.position = currentPosition;
				CurrentLane = TargetLane;
				SwitchingLanes = false;
			} else {
				float zPosition = Mathf.Lerp(GetPositionFromLane(CurrentLane), GetPositionFromLane(TargetLane), LaneSwitchCounter / LaneSwitchTime);
				Vector3 currentPosition = transform.position;
				currentPosition.z = zPosition;
				transform.position = currentPosition;
			}
		}

		if((transform.position.x - LastDistanceWherePointsWereGiven) > DistanceBetweenPoints) {
			GameController.IncreaseScore(1);
			LastDistanceWherePointsWereGiven = transform.position.x;
		}
	}



	void CheckInput() {
		float xAxis = Input.GetAxis("Horizontal");

		if(postPowerCounter > 0f) {
			postPowerCounter -= Time.deltaTime;
			if(postPowerCounter < 0)
				postPowerCounter = 0f;
		}

		if(Input.touchCount > 1) {
			if(PowerController.AttemptPower()) {
				postPowerCounter = .1f;
			}
		} else if(!InputPressed && !SwitchingLanes) {

			if( postPowerCounter == 0f && Input.touchCount > 0) {
				if(delayCounter < 0f) {
					delayCounter = .035f;
				}

				if(delayCounter > 0) {
					delayCounter -= Time.deltaTime;
					if(delayCounter < 0)
						delayCounter = 0f;
				}

				if(delayCounter == 0f) {
					 if(Input.GetMouseButton(0) && !SwitchingLanes && Input.mousePosition.y < Screen.height - 100f && Input.mousePosition.y > 80f) {
						if(Input.mousePosition.x > Screen.width/2f) {
							SwitchLanes(1);
						}else {
							SwitchLanes(-1);
						}
					}
				}
			}
		}

		if(xAxis < -.1f) {
			SwitchLanes(-1);
		} else if(xAxis > .1f) {
			SwitchLanes(1);
		}

		if(RoughlyEqual(xAxis, 0f, .02f)) {
			InputPressed = false;
		}
	}

	void SwipeHappened(SwipeDetector.SwipeType type) {
		if(!InputPressed && !SwitchingLanes) {
			if(type == SwipeDetector.SwipeType.Left) {
				SwitchLanes(-1);
			} else if(type == SwipeDetector.SwipeType.LeftLong) {
				SwitchLanes(-2);
			}else if(type == SwipeDetector.SwipeType.Right) {
				SwitchLanes(1);
			} else if(type == SwipeDetector.SwipeType.RightLong) {
				SwitchLanes(2);
			}
		}
	}

	void SwitchLanes(int direction) {
		if( (CurrentLane == Lane.Left && direction < 0) || (CurrentLane == Lane.Right && direction > 0) ) {
			// User is trying to move to a lane that doesn't exist... tisk tisk
		} else {
			int targetLane = (int)CurrentLane + direction;
			if(targetLane < -1)
				targetLane = -1;
			else if(targetLane > 1)
				targetLane = 1;
			TargetLane = (Lane)(targetLane);
			LaneSwitchCounter = 0f;
			SwitchingLanes = true;
			InputPressed = true;
			delayCounter = -1f;
		}
	}

	void UpdateObstacleMaterials() {
		for(int i = 0; i < ObstacleMaterials.Length; i++) {
			float danger = DangerLevelForObstacle(ObstacleMomentums[i]);
			Color color;
			if(danger > 3.5f) {
				color = Color.black;
			} else if(danger < 1f) {
				color = new Color(.3f, 0f, 0f);
			} else if(danger < 2f){
				color = Color.black;
			} else {
				color = Color.black;
			}
			ObstacleMaterials[i].EnableKeyword ("_EMISSION");
			ObstacleMaterials[i].SetColor("_EmissionColor", color);
		}
	}

	float DangerLevelForObstacle(float requiredMomentum) {
		return DetermineMomentum() / requiredMomentum;
	}

	public static float GetPositionFromLane(Lane lane) {
		if(lane == Lane.Left)
			return LeftLanePosition;
		if(lane == Lane.Middle)
			return MiddleLanePosition;
		return RightLanePosition;
	}

	void SetExtraGobbos() {
		for(int i = 0; i < ExtraGobbos.Length; i++) {
			ExtraGobbos[i].SetActive(i < CurrentGoblins);
		}
	}

	void DestroyExtraGobbos() {
		for(int i = 0; i < ExtraGobbos.Length; i++) {
			if(ExtraGobbos[i].activeSelf) {
				GameObject deathParticles = (GameObject)Instantiate(GoblinDeathParticles, ExtraGobbos[i].transform.position, Quaternion.identity);
				deathParticles.transform.Rotate(new Vector3(0f, 270f, 0f));
				ExtraGobbos[i].SetActive(false);
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Wall")) {
			Obstacle obstacle = other.gameObject.GetComponent<Obstacle>();
			
			switch(obstacle.gameObject.name)
			{
				// Small
				case "obstacle_barricade(Clone)":
					soundSource.PlayOneShot(gobboSmashSmall);
					break;
				case "obstacle_block(Clone)":
					soundSource.PlayOneShot(gobboSmashSmall);
					break;
				case "obstacle_rockSmall(Clone)":
					soundSource.PlayOneShot(gobboSmashSmall);
					break;
				case "obstacle_stalagmite(Clone)":
					soundSource.PlayOneShot(gobboSmashSmall);
					break;
				// Medium
				case "obstacle_block_mega(Clone)":
					soundSource.PlayOneShot(gobboSmashMed);
					break;
				case "obstacle_hut(Clone)":
					soundSource.PlayOneShot(gobboSmashMed);
					break;
				case "obstacle_rockMedium(Clone)":
					soundSource.PlayOneShot(gobboSmashMed);
					break;
				case "obstacle_stalagmite_column(Clone)":
					soundSource.PlayOneShot(gobboSmashMed);
					break;
				// Large
				case "obstacle_rockLarge(Clone)":
					soundSource.PlayOneShot(gobboSmash);
					break;
				case "obstacle_stalagmite_column_giant(Clone)":
					soundSource.PlayOneShot(gobboSmash);
					break;
				case "Wall one(Clone)":
					soundSource.PlayOneShot(gobboSmash);
					break;
				case "Wall two(Clone)":
					soundSource.PlayOneShot(gobboSmash);
					break;
				case "Wall three(Clone)":
					soundSource.PlayOneShot(gobboSmash);
					break;
			}

			if(DetermineMomentum() > obstacle.RequiredMomentum) {
				obstacle.Break();
				float amountOver = DangerLevelForObstacle(obstacle.RequiredMomentum);
				float speedModifer = (1f - (obstacle.RequiredMomentum * 1.5f) / DetermineMomentum());
				if(speedModifer < .25f)
					speedModifer = .25f;

				if(obstacle.IsGold)
					GameController.IncreaseMultiplier();
				// GameController.IncreaseScore((int)Mathf.Pow (obstacle.RequiredMomentum, 1.5f));
				if(other.gameObject.CompareTag("Wall")) {
					GameController.IncreaseMultiplier();
					LevelController.WallDestroyed();
					CreateRagdolls();
					CurrentGoblins = 0;
					SetExtraGobbos();
					if(speedModifer < .75f)
						speedModifer = .75f;
				}

				Speed *= speedModifer;
			} else {
				ImpactSpeed = Speed;
				Speed = 0;
				BonusSpeed = 0;
				GameObject deathParticles = (GameObject)Instantiate(GoblinDeathParticles, Model.transform.position, Quaternion.identity);
				deathParticles.transform.Rotate(new Vector3(0f, 270f, 0f));
				Destroy(Model);
				DestroyExtraGobbos();
				Dead = true;
				UpdateSpeed(false);
				GameController.GameOver();
			}
		} else if(other.gameObject.CompareTag("Powerup")) {
			// Momentum powerup
			soundSource.PlayOneShot(gobboOnGobbo);
			if (CurrentGoblins < MaxGoblins) {
				CurrentGoblins += 1;
				SetExtraGobbos();
			}
			Destroy(other.gameObject);
		} else if(other.gameObject.CompareTag("SpeedPowerup")) {
			//BonusSpeed += SpeedBoostBonus;
			soundSource.PlayOneShot(gobboOnGobbo);
			SpeedBoost();
			//Speed *= .9f;
			Destroy(other.gameObject);
			GameObject obj = (GameObject)Instantiate(SpeedPickupExplosion, transform.position, Quaternion.identity);
			obj.transform.parent = transform;
		} else if(other.gameObject.CompareTag("PowerBoost")) {
			//MomentumBonuses--;
			soundSource.PlayOneShot(gobboOnGobbo);
			//GameController.HighlightPower(false);
			MomentumBonuses ++;
			Destroy(other.gameObject);
			GameObject obj = (GameObject)Instantiate(MultiplierPickupExplosion, transform.position, Quaternion.identity);
			obj.transform.parent = transform;
		} else if(other.gameObject.CompareTag("Runner")) {
			other.gameObject.GetComponent<RunningGobbo>().Death(transform);
			soundSource.PlayOneShot(gobboOnGobbo);
			PowerController.EnablePowerup(true);
			GameController.IncreaseScore(10);
		}
	}

	void SpeedBoost() {
		Speed += SpeedBoostBonus;
		//CancelInvoke("SpeedBoostOver");
		//Invoke ("SpeedBoostOver", SpeedBoostTime);
	}

	public void SpeedPower() {
		Speed += SpeedBoostBonus * 2f;
	}

	void SpeedBoostOver() {
		BonusSpeed = 0f;
	}

	public float DetermineMomentum() {
		return (Speed + BonusSpeed) * (1 + MomentumBonuses * .1f + CurrentGoblins * GoblinPowerBonus);
	}

	public void Bomb() {
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		foreach(GameObject obj in obstacles) {

			if(Vector3.Distance(transform.position, obj.transform.position) < BombDistance) {
				Obstacle ob = obj.GetComponent<Obstacle>();
				ob.Break();
			}
		}
	}

	public void AllGold()
	{
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		foreach(GameObject obj in obstacles)
		{
			if(Vector3.Distance(transform.position, obj.transform.position) < BombDistance)
				{
				Obstacle ob = obj.GetComponent<Obstacle>();
				ob.GoGold();
			}
		}
	}



	public static bool RoughlyEqual(float a, float b, float threshold) {
		return (Mathf.Abs(a-b)< threshold);
	}

	public float GetSpeed() {
		return Speed + BonusSpeed;
	}

	float GetTilt() {
		Quaternion referenceRotation = Quaternion.identity;
		Quaternion deviceRotation = DeviceRotation.Get();
		/*Quaternion eliminationOfXY = Quaternion.Inverse(
			Quaternion.FromToRotation(referenceRotation * Vector3.forward, 
		                          deviceRotation * Vector3.forward)
			); */
		return deviceRotation.eulerAngles.y;
	}

	void CreateRagdolls() {
		float xPosition = -.75f;
		for(int i = 0; i < CurrentGoblins; i++) {
			Vector3 creationPosition = transform.position;
			creationPosition.x += xPosition;
			GameObject rag = (GameObject)Instantiate(RagdollPrefab, creationPosition, Quaternion.identity);
			Transform pelvis = rag.transform.FindChild("pelvis");
			Vector3 forceAmount = new Vector3(Random.Range (60f, 80f), Random.Range(3f, 12f), Random.Range (-16f, 16f));
			forceAmount *= 100f;
			Rigidbody[] rigidBodies = rag.GetComponentsInChildren<Rigidbody>();
			foreach(Rigidbody r in rigidBodies) {
				r.AddForce(forceAmount);
			}


			xPosition -= .75f;
		}
	}

}
