using UnityEngine;
using System.Collections;

public class SpriteLooper : MonoBehaviour {
	public Material material;
	public Texture[] Sprites;
	public float AnimationSpeed;
	private int index;
	private float currentTime;
	private float nextFrameTime;
	private float animationDelta {get {return AnimationSpeed / (float)Sprites.Length;}}

	// Use this for initialization
	void Start () {
		index = 0;
		currentTime = 0;
		nextFrameTime = animationDelta;
		SetSprite();
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;
		if(currentTime >= nextFrameTime) {
			index ++;
			if(index >= Sprites.Length) {
				index = 0;
				currentTime = 0;
				nextFrameTime = animationDelta;
			} else {
				nextFrameTime += animationDelta;
			}
			SetSprite();
		}
	}

	void SetSprite() {
		material.SetTexture("_MainTex", Sprites[index]);
	}
}
