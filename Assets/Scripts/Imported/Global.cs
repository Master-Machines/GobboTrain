using UnityEngine;
using System.Collections;
using System.Reflection;

// Variables that are persisted
public class Global {

	public int currency = 0;
	public int highScore = 0;
	public bool isPaused = false;
	public bool tiltEnabled = false;

	public const int TotalNumberOfUpgrades = 13;

	// Selected Powers
	public bool IncreasedLowerAccel = false;
	public bool IncreaseHigherAccel = false;
	public bool IncreaseMaxGobbos = false;
	public bool ExtraGobboWhenBreakingThroughWall = false;
	public bool LessSpeedReduction = false;
	public bool ExtraGobboPickups = false;
	public bool ExtraPowerOrbs = false;
	public bool ExtraSpeedOrbs = false;
	public bool ExtraSpeedFromSpeedPowerup = false;
	public bool ExtraPowerFromGobbos = false;
	public bool WeakerWalls = false;
	public bool IncreasedTopSpeed = false;
	public bool IncreasedDistanceBetweenWalls = false;

	// Unlocked powers
	public bool IncreasedLowerAccelS = false;
	public bool IncreaseHigherAccelS = false;
	public bool IncreaseMaxGobbosS = false;
	public bool ExtraGobboWhenBreakingThroughWallS = false;
	public bool LessSpeedReductionS = false;
	public bool ExtraGobboPickupsS = false;
	public bool ExtraPowerOrbsS = false;
	public bool ExtraSpeedOrbsS = false;
	public bool ExtraSpeedFromSpeedPowerupS = false;
	public bool ExtraPowerFromGobbosS = false;
	public bool WeakerWallsS = false;
	public bool IncreasedTopSpeedS = false;
	public bool IncreasedDistanceBetweenWallsS = false;
	
	public static Global Instance = new Global();
	private static bool loaded = false;
	
	public void Init() {
		if(!loaded) {
			Instance.Load();
			loaded = true;
		}
	}

	const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
	
	public void Save() {
		FieldInfo[] props = Instance.GetType().GetFields(flags);
		foreach (FieldInfo f in props) {
			if(f.FieldType == typeof(System.Int32)) {
				PlayerPrefs.SetInt(f.Name, (int)f.GetValue(Instance));
			} else if(f.FieldType == typeof(System.Boolean)) {
				// if false, save 0, otherwise save 1
				PlayerPrefs.SetInt(f.Name, (bool)f.GetValue(Instance) ? 1 : 0);
			} else if(f.FieldType == typeof(System.String)) {
				PlayerPrefs.SetString(f.Name, f.GetValue(Instance).ToString());
			} else if(f.FieldType == typeof(System.Single)){
				PlayerPrefs.SetFloat(f.Name, (float)f.GetValue(Instance));
			}
		}
	}
	
	public void Load() {
		FieldInfo[] props = Instance.GetType().GetFields(flags);
		foreach (FieldInfo f in props) {
			if(f.FieldType == typeof(System.Int32)) {
				f.SetValue(Instance, PlayerPrefs.GetInt(f.Name));
			} else if(f.FieldType == typeof(System.Boolean)) {
				// if false, save 0, otherwise save 1
				int b = PlayerPrefs.GetInt(f.Name);
				f.SetValue(Instance, (b == 1) ? true : false);
			} else if(f.FieldType == typeof(System.String)) {
				f.SetValue(Instance, PlayerPrefs.GetString(f.Name));
			} else if(f.FieldType == typeof(System.Single)){
				f.SetValue(Instance, PlayerPrefs.GetFloat(f.Name));
			}
		}
	}

	public void DeselectAllUpgrades() {
		IncreasedLowerAccel = false;
		IncreaseHigherAccel = false;
		IncreaseMaxGobbos = false;
		ExtraGobboWhenBreakingThroughWall = false;
		LessSpeedReduction = false;
		ExtraGobboPickups = false;
		ExtraPowerOrbs = false;
		ExtraSpeedOrbs = false;
		ExtraSpeedFromSpeedPowerup = false;
		ExtraPowerFromGobbos = false;
		WeakerWalls = false;
		IncreasedTopSpeed = false;
		IncreasedDistanceBetweenWalls = false;
	}

	public int NumberOfSelectedUpgrades() {
		int total = 0;
		total += IncreasedLowerAccel ? 1 : 0;
		total += IncreaseHigherAccel ? 1 : 0;
		total += IncreaseMaxGobbos ? 1 : 0;
		total += ExtraGobboWhenBreakingThroughWall ? 1 : 0;
		total += LessSpeedReduction ? 1 : 0;
		total += ExtraGobboPickups ? 1 : 0;
		total += ExtraPowerOrbs ? 1 : 0;
		total += ExtraSpeedOrbs ? 1 : 0;
		total += ExtraSpeedFromSpeedPowerup ? 1 : 0;
		total += ExtraPowerFromGobbos ? 1 : 0;
		total += WeakerWalls ? 1 : 0;
		total += IncreasedTopSpeed ? 1 : 0;
		total += IncreasedDistanceBetweenWalls ? 1 : 0;
		return total;
	}

	public int NumberOfUnlockedUpgrades() {
		int total = 0;
		total += IncreasedLowerAccelS ? 1 : 0;
		total += IncreaseHigherAccelS ? 1 : 0;
		total += IncreaseMaxGobbosS ? 1 : 0;
		total += ExtraGobboWhenBreakingThroughWallS ? 1 : 0;
		total += LessSpeedReductionS ? 1 : 0;
		total += ExtraGobboPickupsS ? 1 : 0;
		total += ExtraPowerOrbsS ? 1 : 0;
		total += ExtraSpeedOrbsS ? 1 : 0;
		total += ExtraSpeedFromSpeedPowerupS ? 1 : 0;
		total += ExtraPowerFromGobbosS ? 1 : 0;
		total += WeakerWallsS ? 1 : 0;
		total += IncreasedTopSpeedS ? 1 : 0;
		total += IncreasedDistanceBetweenWallsS ? 1 : 0;
		return total;
	}

	public int CostToPurchaseNextPowerup() {
		int num = NumberOfUnlockedUpgrades();
		int cost = 0;
		if(num < 5) {
			cost = 250 * (num + 1);
		} else if(num < 10) {
			cost = 1000 * (num - 3);
		} else {
			cost = 5000 + 5000 * (num - 8);
		}
		return cost;
	}

	public void SelectUpgradeAtIndex(int index, bool selected) {
		switch (index) {
		case 0:
			IncreasedLowerAccel = selected;
			break;
		case 1:
			IncreaseHigherAccel = selected;
			break;
		case 2:
			IncreaseMaxGobbos = selected;
			break;
		case 3:
			ExtraGobboWhenBreakingThroughWall = selected;
			break;
		case 4:
			LessSpeedReduction = selected;
			break;
		case 5:
			ExtraGobboPickups = selected;
			break;
		case 6:
			ExtraPowerOrbs = selected;
			break;
		case 7:
			ExtraSpeedOrbs = selected;
			break;
		case 8:
			ExtraSpeedFromSpeedPowerup = selected;
			break;
		case 9:
			ExtraPowerFromGobbos = selected;
			break;
		case 10:
			WeakerWalls = selected;
			break;
		case 11:
			IncreasedTopSpeed = selected;
			break;
		case 12:
			IncreasedDistanceBetweenWalls = selected;
			break;
		}
	}

	public void PurchaseUpgradeAtIndex(int index) {
		switch (index) {
		case 0:
			IncreasedLowerAccelS = true;
			break;
		case 1:
			IncreaseHigherAccelS = true;
			break;
		case 2:
			IncreaseMaxGobbosS = true;
			break;
		case 3:
			ExtraGobboWhenBreakingThroughWallS = true;
			break;
		case 4:
			LessSpeedReductionS = true;
			break;
		case 5:
			ExtraGobboPickupsS = true;
			break;
		case 6:
			ExtraPowerOrbsS = true;
			break;
		case 7:
			ExtraSpeedOrbsS = true;
			break;
		case 8:
			ExtraSpeedFromSpeedPowerupS = true;
			break;
		case 9:
			ExtraPowerFromGobbosS = true;
			break;
		case 10:
			WeakerWallsS = true;
			break;
		case 11:
			IncreasedTopSpeedS = true;
			break;
		case 12:
			IncreasedDistanceBetweenWallsS = true;
			break;
		}
	}
	
	
}
