using UnityEngine;
using System.Collections;

public class LimbStretchHack : MonoBehaviour {
	private Vector3 startPosition;
	
	public void Start()
	{
		startPosition = transform.localPosition;
	}
	
	public void LateUpdate()
	{
		transform.localPosition = startPosition;
	} 
}