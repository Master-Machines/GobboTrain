using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	public float Size;
	public float RequiredMomentum;
	public float yAdjustment;
	public int CurrencyBonus;

	public GameObject ExplosionParticles;

	// Use this for initialization
	IEnumerator Start () {
		transform.Translate(new Vector3(0f, yAdjustment, 0f));
		yield return new WaitForSeconds(10f);
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Break() {
		if(ExplosionParticles != null)
			Instantiate(ExplosionParticles, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
