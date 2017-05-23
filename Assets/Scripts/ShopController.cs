using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {
	public GameObject ScrollContent;
	public GameObject ItemPrefab;

	public Button[] AbilityButtons;
	public Button[] SpecialtyButtons;

	public PoewrDialogController PowerDialog;

	public ColorBlock SelectedColor;
	public ColorBlock StandardColor;

	public Text CurrencyDisplay;
    public int TotalCurrency;
    public int SessionCurrency;

	void Start () {
		UpdateSelectedAbilities();
        TotalCurrency = PlayerPrefs.GetInt("currencyPref");
        CurrencyDisplay.text = TotalCurrency.ToString() + " G";
    }

	void Update () {
	
	}

	public void UpdateSelectedAbilities() {

		int ability = Global.Instance.SelectedPowerup - 1;
		int specialty = Global.Instance.SelectedSpecialty - 1;
		foreach(Button abilityButton in AbilityButtons) {
			abilityButton.colors = StandardColor;
		}
		foreach(Button specialtyButton in SpecialtyButtons) {
			specialtyButton.colors = StandardColor;
		}

		if(ability > -1)
			AbilityButtons[ability].colors = SelectedColor;
		if(specialty > -1)
			SpecialtyButtons[specialty].colors = SelectedColor;

		CurrencyDisplay.text = Global.Instance.currency + " c";
	}

	public void AbilitySelected(int index) {
		PowerDialog.gameObject.SetActive(true);
		PowerDialog.Setup(index, true);
	}

	public void SpecialtySelected(int index) {
		PowerDialog.gameObject.SetActive(true);
		PowerDialog.Setup(index, false);
	}

	public void Leave() {
		Application.LoadLevel("GameScene");
	}
}
