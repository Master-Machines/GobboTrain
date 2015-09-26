using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	public float Size;
	public float RequiredMomentum;
	public float yAdjustment;
	public int CurrencyBonus;
	public bool AllowGold = true;
	public Material StandardMaterial;
	public Material GoldMaterial;
	public bool IsGold {get; private set;}
	public GameObject GoldParticles;

	public GameObject ExplosionParticles;
	private const float GoldChance = 0.07f;
	public static bool PureGold = false;

	// Use this for initialization
	IEnumerator Start () {
		if(AllowGold && (Random.Range (0f, 1f) < GoldChance || PureGold)) {
			GoGold();
		}

		transform.Translate(new Vector3(0f, yAdjustment, 0f));
		yield return new WaitForSeconds(10f);
		Destroy(gameObject);
	}

	public void GoGold() {
		if(!IsGold) {
			Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
			foreach(Renderer renderer in renderers) {
				renderer.material = GoldMaterial;
			}

			GameObject goldP = (GameObject)Instantiate(GoldParticles, transform.position, Quaternion.identity);
			goldP.GetComponent<ParticleSystem>().emissionRate = RequiredMomentum;
			goldP.transform.SetParent(transform);
			goldP.transform.Translate(Vector3.up);
			RequiredMomentum *= .8f;
			IsGold = true;
		}
	}

	public void Break() {
		if(ExplosionParticles != null)
			Instantiate(ExplosionParticles, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
