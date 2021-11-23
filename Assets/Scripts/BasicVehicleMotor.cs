using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicVehicleMotor : MonoBehaviour {

	public float maxSpeed = 20;
	public float maxBoostSpeed = 40;
	public float accelerationPower = 30;
	public float boostPower = 50;
	public float maxBrakePower = 3;
	public float maxBrakeTime = 3;
	public float steeringPower = 3.5f;
	public float driftPower = .95f;
	public float minSpeedToRotate;
	public float softRotationInertia = 8;
	
	public float boostInput;
	public float accelerationInput;
	public float steeringInput;

	public Rigidbody2D rigidbody;
	public TrailRenderer trail;

	public float rotation;
	public bool isBoosting;

	public BoostManager boostManager;

	private void Awake() {
		if (rigidbody == null) rigidbody = GetComponent<Rigidbody2D>();
		if (trail == null) trail = GetComponentInChildren<TrailRenderer>();
	}

	public void FixedUpdate() {
		UpdateEngine();
		AdjustDrift();
		UpdateSteering();
	}

	public bool CheckBoost()
    {
		return boostManager.currentBoostAmount > 0 && boostManager.canBoost;
    }

	private void UpdateEngine() {
		float forwardSpeed = Vector2.Dot(transform.up, rigidbody.velocity);

		if (accelerationInput <= 0 || forwardSpeed <= 0) boostInput = 0;

		if(boostManager != null)
        {
			if (!CheckBoost())
			{
				boostInput = 0;
			}

			if (isBoosting)
			{
				boostManager.DischargeBoost();
			}
			else
			{
				boostManager.RechargeBoost();
			}
		}
		
		var maxSpeed = Mathf.Lerp(this.maxSpeed, this.maxBoostSpeed, boostInput);
		var accelerationPower = Mathf.Lerp(this.accelerationPower, boostPower, boostInput);
		
		var shouldBoost = boostInput > 0;

		if (isBoosting && !shouldBoost) {
			trail.enabled = false;
			trail.emitting = false;
			trail.Clear();
		} else if (!isBoosting && shouldBoost) {
			trail.emitting = true;
			trail.enabled = true;
		}

		isBoosting = shouldBoost;
		
		if (accelerationInput > 0 && rigidbody.velocity.sqrMagnitude >= maxSpeed * maxSpeed) return;
		if (accelerationInput > 0 && forwardSpeed >= maxSpeed) return;
		if (accelerationInput < 0 && forwardSpeed <= -maxSpeed / 2f) return;

		if (accelerationInput == 0) rigidbody.drag = Mathf.MoveTowards(rigidbody.drag, maxBrakePower, Time.fixedDeltaTime * maxBrakeTime);
		else rigidbody.drag = 0;

		Vector2 engineForce = transform.up * (accelerationInput * accelerationPower);
		rigidbody.AddForce(engineForce, ForceMode2D.Force);
	}

	private void UpdateSteering() {
		float speed = rigidbody.velocity.magnitude;
		float rotationInertiaFactor = speed <= minSpeedToRotate ? 0 : speed / softRotationInertia;
		rotationInertiaFactor = Mathf.Clamp01(rotationInertiaFactor);

		rotation -= steeringInput * steeringPower * rotationInertiaFactor;
		rigidbody.MoveRotation(rotation);
	}

	private void AdjustDrift() {
		Vector2 forwardVelocity = transform.up * Vector2.Dot(rigidbody.velocity, transform.up);
		Vector2 rightVelocity = transform.right * Vector2.Dot(rigidbody.velocity, transform.right);

		rigidbody.velocity = forwardVelocity + rightVelocity * driftPower;
	}
}
