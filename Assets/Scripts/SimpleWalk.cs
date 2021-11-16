using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWalk : MonoBehaviour {

	public Transform[] waypoints;
	public Transform currentTarget;
	public Rigidbody2D rigidbody;
	public int currentIndex;
	public float speed = 1;

	public void Start() {
		if (waypoints.Length > 0) {
			currentTarget = waypoints[0];
			currentIndex = 0;
		}
		
		if (rigidbody == null) rigidbody = GetComponent<Rigidbody2D>();
	}

	public void EfficientUpdate() {
		// transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
		if (!currentTarget) {
			if (waypoints.Length > 0) {
				currentTarget = waypoints[0];
				currentIndex = 0;
			} else return;
		}
		rigidbody.velocity = (currentTarget.position - transform.position).normalized * speed;
		if ((transform.position - currentTarget.position).magnitude < 0.1f) {
			currentIndex += 1;
			if (currentIndex == waypoints.Length) currentIndex = 0;
			currentTarget = waypoints[currentIndex];
		}
	}
}