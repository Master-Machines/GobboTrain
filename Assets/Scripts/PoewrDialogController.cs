using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PoewrDialogController : MonoBehaviour {
	public const int PurchaseCost = 100;
	public const int UpgradeCost = 500;

	public Text UpgradeButtonText;
	public Text UseButtonText;
	public Button UpgradeButton;
	public Button UseButton;


	public Text Title;
	public Text Description;

	public Text UpgradeT;
	public Text UpgradeDescription;

	public ShopController ShopController;

	private int CurrentPurchaseCost;
	private bool CurrentIsAbility;
	private int CurrentIndex;

	void Start () {
		Global.Instance.currency = 1000;
	}

	void Update () {
	
	}

	public void Setup(int powerIndex, bool isAbility) {
		CurrentIsAbility = isAbility;
		CurrentIndex = powerIndex;
		int level = 0;

		if(isAbility) {
			if(powerIndex == 1) {
				level = Global.Instance.Powerup1Level;
				Title.text = "shockwave";
				Description.text = "shatters nearby obstacles";
				UpgradeDescription.text = "damages walls";
			} else if(powerIndex == 2) {
				level = Global.Instance.Powerup2Level;
				Title.text = "boost";
				Description.text = "gives a boost in speed";
				UpgradeDescription.text = "temporarily increases max speed";
			} else if(powerIndex == 3) {
				Title.text = "gold rush";
				Description.text = "makes all nearby obstacles turn into gold obstacles";
				UpgradeDescription.text = "extends the distance of gold rush";
				level = Global.Instance.Powerup3Level;
			}
		} else {
			if(powerIndex == 1) {
				level = Global.Instance.Specialty1Level;
				Title.text = "tank";
				Description.text = "hitting obstacles does not decrease speed as much";
				UpgradeDescription.text = "the goblin train has a chance to not break after smashing through a wall";
			} else if(powerIndex == 2) {
				level = Global.Instance.Specialty2Level;
				Title.text = "merchant";
				Description.text = "more gold obstacles";
				UpgradeDescription.text = "no decrease in speed when breaking gold obstacles";
			} else if(powerIndex == 3) {
				level = Global.Instance.Specialty3Level;
				Title.text = "runner";
				Description.text = "gain speed faster";
				UpgradeDescription.text = "higher top speed";
			}
		}

		CurrentPurchaseCost = 0;
		if(level == 0) {
			// The powerup isn't unlocked!
			UpgradeButton.enabled = false;
			UseButtonText.text = "Purchase  \n" + PurchaseCost + " c";
			CurrentPurchaseCost = PurchaseCost;
			UpgradeButton.gameObject.SetActive(false);
		} else if(level == 1) {
			UpgradeButton.enabled = true;
			UpgradeButtonText.text = "upgrade \n" + UpgradeCost + " c";
			UseButtonText.text = "use";
			UpgradeButton.gameObject.SetActive(true);
		} else {
			// User has the upgrade
			UseButtonText.text = "use";
			UpgradeButton.gameObject.SetActive(false);
		}
	}

	public void Use() {
		bool isEquiping = true;
		if(CurrentPurchaseCost == 0) {
			// Player wants to equip the item.
		} else if(Global.Instance.currency >= CurrentPurchaseCost){
			// Player wants to buy and equip the item.
			Global.Instance.currency -= CurrentPurchaseCost;
			SetUpgradeLevel(CurrentIsAbility, CurrentIndex, 1);
		} else {
			isEquiping = false;
		}

		if(isEquiping) {
			if(CurrentIsAbility) {
				Global.Instance.SelectedPowerup = CurrentIndex;
			} else {
				Global.Instance.SelectedSpecialty = CurrentIndex;
			}
			Close ();
		}
	}

	public void Upgrade() {
		if(Global.Instance.currency >= UpgradeCost) {
			Global.Instance.currency -= UpgradeCost;
			SetUpgradeLevel(CurrentIsAbility, CurrentIndex, 2);
			Close ();
		}
	}

	public void Close() {
		ShopController.UpdateSelectedAbilities();
		gameObject.SetActive(false);
	}

	void SetUpgradeLevel(bool isAbility, int index, int level) {
		if(isAbility) {
			if(index == 1) {
				Global.Instance.Powerup1Level = level;
			} else if(index == 2) {
				Global.Instance.Powerup2Level = level;
			} else if(index == 3) {
				Global.Instance.Powerup3Level = level;
			}
		} else {
			if(index == 1) {
				Global.Instance.Specialty1Level = level;
			} else if(index == 2) {
				Global.Instance.Specialty2Level = level;
			} else if(index == 3) {
				Global.Instance.Specialty3Level = level;
			}
		}
	}
}
