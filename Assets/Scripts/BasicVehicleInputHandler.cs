﻿	using System;
using System.Collections.Generic;
using UnityEngine;

public class BasicVehicleInputHandler : MonoBehaviour {

	public ComboMeter comboMeter;
	public float dmgMinInterval = .1f;
	public float minSpeedToDmg = 1f;
	public int minSmashDmg = 10;
	public int maxSmashDmg = 100;
	public float dustThreshold = .9f;

	public ParticleSystem dustPlayer;
	public BasicVehicleMotor motor;
	public Transform frame;
	public Transform[] wheels;

	private bool dustMark;
	private Rigidbody2D _rigidbody;
	private Dictionary<int, float> _dmgInfo = new Dictionary<int, float>();
	private List<int> _tempList = new List<int>();

	private void Awake() {
		_rigidbody = GetComponent<Rigidbody2D>();
		if (motor == null) motor = GetComponent<BasicVehicleMotor>();
	}

	private void Update() {
		float verticalInput = Input.GetAxis("Vertical");
		float horizontalInput = Input.GetAxis("Horizontal");
		float boostInput = Input.GetButton("Fire2") ? 1 : 0;

		motor.accelerationInput = verticalInput;
		motor.steeringInput = horizontalInput;
		motor.boostInput = boostInput;
		
		if (Mathf.Abs(horizontalInput) >= dustThreshold) MakeDust();
		else StopDust();

		float wheelRotation = -horizontalInput;

		if (wheels != null) {
			foreach (var wheel in wheels) {
				Vector3 rot = frame.eulerAngles;
				float zRot = Mathf.LerpUnclamped(0, 45, wheelRotation);
				rot.z += zRot;
				wheel.eulerAngles = rot;
			}
		}
		
		UpdateDmgInfo();
	}

	private void UpdateDmgInfo() {
		
		_tempList.Clear();
		
		foreach (var info in _dmgInfo) {
			if ((Time.timeSinceLevelLoad - info.Value) >= dmgMinInterval) _tempList.Add(info.Key);
		}

		foreach (var id in _tempList) _dmgInfo.Remove(id);
	}

	private void Hit(Collision2D other) {
		if (!other.collider.TryGetComponent<HealthScript>(out var health)) return;
		Debug.Log("hit");
		var id = health.GetInstanceID();
		if (_dmgInfo.ContainsKey(id) && (Time.timeSinceLevelLoad - _dmgInfo[id]) < dmgMinInterval) return;
		_dmgInfo[id] = Time.timeSinceLevelLoad;
		var contact = other.GetContact(0);
		// var dir = (contact.point - (Vector2) transform.position).normalized;
		// var vel = _rigidbody.velocity;
		var vel = contact.relativeVelocity;
		// var spd = Vector3.Dot(vel, dir);
		var spd = vel.magnitude;
		// print(other.collider.name + " SPD " + spd.ToString("F3"));
		if (spd < minSpeedToDmg) return;
		var dmg = Mathf.Lerp(minSmashDmg, maxSmashDmg, (spd - minSpeedToDmg) / (motor.maxBoostSpeed - minSpeedToDmg));
		// print("DMG " + dmg);
		health.OnDamageTaken((int) dmg, vel.normalized, transform);
		if(health.hp <= 0)
        {
			comboMeter.OnKill();
        }
	}

	public void MakeDust() {
		if (!dustMark) {
			dustPlayer.Play();
			dustMark = true;
		}
	}

	public void StopDust() {
		if (dustMark) {
			dustPlayer?.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			dustMark = false;
		}
	}

	private void OnCollisionEnter2D(Collision2D other) => Hit(other);
	
	private void OnCollisionStay2D(Collision2D other) => Hit(other);

	// private void OnTriggerEnter2D(Collider2D other) => Hit(other);

	// private void OnTriggerStay2D(Collider2D other) => Hit(other);
}