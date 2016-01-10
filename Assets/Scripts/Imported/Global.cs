using UnityEngine;
using System.Collections;
using System.Reflection;

// Variables that are persisted
public class Global {

	public int currency = 0;
	public int highScore = 0;
	public bool isPaused = false;
	public bool tiltEnabled = false;

	public int SelectedPowerup = 1;
	public int SelectedSpecialty = 0;

	// shockwave 
	public int Powerup1Level = 1;
	// boost
	public int Powerup2Level = 0;
	// goldrush
	public int Powerup3Level = 0;
	// tank
	public int Specialty1Level = 0;
	// merchant
	public int Specialty2Level = 0;
	// runner
	public int Specialty3Level = 0; 

	
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

	
}
