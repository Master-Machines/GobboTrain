using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SwipeDetector : MonoBehaviour {

	public enum SwipeType{
		Up,
		UpLong,
		Down,
		DownLong,
		Left,
		LeftLong,
		Right,
		RightLong
	}

	public List<Action<SwipeType>> SwipeEvents = new List<Action<SwipeType>>();

	private float maxSwipeTime = 1f;
	private float minSwipeTime = .065f;
	private float minShortSwipe  = 0.2f;
	private float minLongSwipe = 1f;

	private float fingerStartTime = 0f;
	private Vector2 fingerStartPos;
	private bool isSwipe = false;

	void Start () {

	}

	void UpdateOff(){//Update () {
		if (Input.touchCount > 0 && !TimeController.IsPaused){
			foreach (Touch touch in Input.touches)
			{
				switch (touch.phase)
				{
				case TouchPhase.Began :
					/* this is a new touch */
					if(!isSwipe) {
						isSwipe = true;
						fingerStartTime = Time.unscaledTime;
						fingerStartPos = touch.position;
					}
					break;
					
				case TouchPhase.Canceled :
					/* The touch is being canceled */
					isSwipe = false;
					break;
				case TouchPhase.Ended :
					
					float gestureTime = Time.unscaledTime - fingerStartTime;
					float gestureDist = (touch.position - fingerStartPos).magnitude;
					int state = 0;
					float inces = InchesFromPixels(gestureDist);
					if(inces > minLongSwipe)
						state = 2;
					else if(inces > minShortSwipe)
						state = 1;
					if (isSwipe && state > 0){
						Vector2 direction = touch.position - fingerStartPos;
						Vector2 swipeType = Vector2.zero;
						
						if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
							// the swipe is horizontal:
							swipeType = Vector2.right * Mathf.Sign(direction.x);
						}else{
							// the swipe is vertical:
							swipeType = Vector2.up * Mathf.Sign(direction.y);
						}
						
						if(swipeType.x != 0.0f){
							if(swipeType.x > 0.0f){
								// MOVE RIGHT
								SwipeHappened(state == 1 ? SwipeType.Right : SwipeType.RightLong);
								fingerStartPos = touch.position;
							}else{
								// MOVE LEFT
								SwipeHappened(state == 1 ? SwipeType.Left : SwipeType.LeftLong);
								fingerStartPos = touch.position;
							}
						}
						
						if(swipeType.y != 0.0f ){
							if(swipeType.y > 0.0f){
								// MOVE UP
								SwipeHappened(SwipeType.Up);
								fingerStartPos = touch.position;
							}else{
								// MOVE DOWN
								SwipeHappened(SwipeType.Down);
								fingerStartPos = touch.position;
							}
						}
						
					}
					break;
				}

			}
		}
	}

	float InchesFromPixels(float pixels) {
		float dpi = Screen.dpi;
		if(dpi == 0)
			dpi = 100f;
		return pixels / (float)dpi;
	}

	void SwipeHappened(SwipeType type) {
		foreach(Action<SwipeType> action in SwipeEvents) {
			action(type);
		}
		isSwipe = false;
	}
}
