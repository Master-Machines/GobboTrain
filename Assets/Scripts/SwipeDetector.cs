using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SwipeDetector : MonoBehaviour {

	public enum SwipeType{
		Up,
		Down,
		Left,
		Right
	}

	public List<Action<SwipeType>> SwipeEvents = new List<Action<SwipeType>>();

	private float maxSwipeTime = 1f;
	private float minSwipeTime = .065f;
	private float minSwipeDist  = 40.0f;

	private float fingerStartTime = 0f;
	private Vector2 fingerStartPos;
	private bool isSwipe = false;

	void Start () {
	
	}

	void Update () {
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
					
				case TouchPhase.Moved :
				case TouchPhase.Ended :
					
					float gestureTime = Time.unscaledTime - fingerStartTime;
					float gestureDist = (touch.position - fingerStartPos).magnitude;
					
					if (isSwipe && (gestureTime > minSwipeTime || touch.phase == TouchPhase.Ended) && gestureDist > minSwipeDist){
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
								SwipeHappened(SwipeType.Right);
								fingerStartPos = touch.position;
							}else{
								// MOVE LEFT
								SwipeHappened(SwipeType.Left);
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

	void SwipeHappened(SwipeType type) {
		foreach(Action<SwipeType> action in SwipeEvents) {
			action(type);
		}
		isSwipe = false;
	}
}
