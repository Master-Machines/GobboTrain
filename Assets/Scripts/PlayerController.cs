using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private float Speed = 0f;
	private float BonusSpeed = 0f;

	public const float LeftLanePosition = 5f;
	public const float MiddleLanePosition = 0f;
	public const float RightLanePosition = -5f;
	private const float ExtraLaneSwitchTime = 0f;
	private const float MinLaneSwitchTime = .1f;
	public float ModerateSpeed = 35f;
	public float MaxSpeed = 55f;
	public const float GodSpeed = 85f;

	private const float SpeedBoostBonus = 15f;
	private const float SpeedBoostTime = 3f;
	private const float BombDistance = 150f;
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

	// Use this for initialization
	void Start () {
		Speed = 10f;
		CurrentLane = Lane.Middle;
		SwipeDetector.SwipeEvents.Add(SwipeHappened);
	}
	
	// Update is called once per frame
	void Update () {
		if(!Dead && !Global.Instance.isPaused) {
			UpdatePosition();
			CheckInput();
			UpdateSpeed(true);
			//UpdateObstacleMaterials();
			if(Input.GetButtonDown("Jump") && Bombs-- > 0) {
				Bomb();
			}
		}
	}

	void UpdateSpeed(bool on) {
		if(Speed < ModerateSpeed) {
			Speed += Time.deltaTime * 10f;
		} else if(Speed < MaxSpeed) {
			Speed += Time.deltaTime * 5f;
		} else if (Speed < GodSpeed) {
			Speed += Time.deltaTime * 1f;
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
		transform.Translate(new Vector3(5f + Speed + BonusSpeed, 0f, 0f) * Time.deltaTime);
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
		if(!InputPressed && !SwitchingLanes) {

			if(!Global.Instance.tiltEnabled) {
				if(xAxis < -.1f) {
					SwitchLanes(-1);
				} else if(xAxis > .1f) {
					SwitchLanes(1);
				} else if(Input.GetMouseButton(0) && !SwitchingLanes && Input.mousePosition.y < Screen.height - 100f) {
					/*if(Input.mousePosition.x > Screen.width/2f) {
						SwitchLanes(1);
					}else {
						SwitchLanes(-1);
					}*/
				}
			} else {
				float tilt = Input.acceleration.x;
				if(tilt < LeftTilt) {
					SwitchLanes(-1);
				} else if(tilt > RightTilt) {
					SwitchLanes(1);
				} else {
					if(CurrentLane == Lane.Left) {
						SwitchLanes(1);
					} else if(CurrentLane == Lane.Right) {
						SwitchLanes(-1);
					}
				}
			}

		}

		if(RoughlyEqual(xAxis, 0f, .02f)) {
			InputPressed = false;
		}
	}

	void SwipeHappened(SwipeDetector.SwipeType type) {
		if(!InputPressed && !SwitchingLanes) {
			if(type == SwipeDetector.SwipeType.Left) {
				SwitchLanes(-1);
			} else if(type == SwipeDetector.SwipeType.Right) {
				SwitchLanes(1);
			}
		}
	}

	void SwitchLanes(int direction) {
		if( (CurrentLane == Lane.Left && direction == -1) || (CurrentLane == Lane.Right && direction == 1) ) {
			// User is trying to move to a lane that doesn't exist... tisk tisk
		} else {
			TargetLane = (Lane)((int)CurrentLane + direction);
			LaneSwitchCounter = 0f;
			SwitchingLanes = true;
			InputPressed = true;
		}
	}

	void UpdateObstacleMaterials() {
		for(int i = 0; i < ObstacleMaterials.Length; i++) {
			float danger = DangerLevelForObstacle(ObstacleMomentums[i]);
			Color color;
			if(danger > 3.5f) {
				color = Color.black;
				//color = new Color(0f, .25f, 0f);
			} else if(danger < 1f) {
				color = new Color(.3f, 0f, 0f);
			} else if(danger < 2f){
				color = Color.black;
				//color = new Color(.25f, .12f, 0f);
			} else {
				color = Color.black;
				//color = new Color(.25f, .25f, 0f);
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
			if(DetermineMomentum() > obstacle.RequiredMomentum) {
				obstacle.Break();
				float amountOver = DangerLevelForObstacle(obstacle.RequiredMomentum);
				float speedModifer = (1f - (obstacle.RequiredMomentum * 1.5f) / DetermineMomentum());
				if(speedModifer < .4f)
					speedModifer = .4f;
				Speed *= speedModifer;
				if(obstacle.IsGold)
					GameController.IncreaseCurrency(obstacle.CurrencyBonus);
				// GameController.IncreaseScore((int)Mathf.Pow (obstacle.RequiredMomentum, 1.5f));
				if(other.gameObject.CompareTag("Wall")) {
					GameController.IncreaseMultiplier();
					LevelController.WallDestroyed();
					CreateRagdolls();
					CurrentGoblins = 0;
					GameController.HighlightPower(false);
					SetExtraGobbos();
				}
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

			if(CurrentGoblins < MaxGoblins) {
				CurrentGoblins += 1;
				GameController.HighlightPower(true);
				SetExtraGobbos();
			}
			Destroy(other.gameObject);
		} else if(other.gameObject.CompareTag("SpeedPowerup")) {
			//BonusSpeed += SpeedBoostBonus;
			SpeedBoost();
			//Speed *= .9f;
			Destroy(other.gameObject);
			GameObject obj = (GameObject)Instantiate(SpeedPickupExplosion, transform.position, Quaternion.identity);
			obj.transform.parent = transform;
		} else if(other.gameObject.CompareTag("PowerBoost")) {
			//MomentumBonuses--;
			//GameController.HighlightPower(false);
			MomentumBonuses ++;
			GameController.HighlightPower(true);
			Destroy(other.gameObject);
			GameObject obj = (GameObject)Instantiate(MultiplierPickupExplosion, transform.position, Quaternion.identity);
			obj.transform.parent = transform;
		}
	}

	void SpeedBoost() {
		Speed += SpeedBoostBonus;
		//CancelInvoke("SpeedBoostOver");
		//Invoke ("SpeedBoostOver", SpeedBoostTime);
	}

	void SpeedBoostOver() {
		BonusSpeed = 0f;
	}

	float DetermineMomentum() {
		return (Speed + BonusSpeed) * (1 + MomentumBonuses * .1f + CurrentGoblins * GoblinPowerBonus);
	}

	void Bomb() {
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		foreach(GameObject obj in obstacles) {

			if(Vector3.Distance(transform.position, obj.transform.position) < BombDistance) {
				Obstacle ob = obj.GetComponent<Obstacle>();
				ob.Break();
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
