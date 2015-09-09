using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {
	public float TimeToDeath;
	IEnumerator Start () {
		yield return new WaitForSeconds(TimeToDeath);
		Destroy(gameObject);
	}
}
