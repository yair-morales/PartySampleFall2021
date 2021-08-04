using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicVehicleMotor : MonoBehaviour {

	public float maxSpeed;
	public float accelerationPower;
	public float maxBrakePower;
	public float maxBrakeTime;
	public float steeringPower;
	public float driftPower;
	public float minSpeedToRotate;
	public float softRotationInertia = 8;

	public float accelerationInput;
	public float steeringInput;
	
	public Rigidbody2D rigidbody;

	public Vector2 currentVelocity;
	public float rotation;

	private void Awake() {
		if (rigidbody == null) rigidbody = GetComponent<Rigidbody2D>();
	}

	public void FixedUpdate() {
		UpdateEngine();
		AdjustDrift();
		UpdateSteering();
	}

	private void UpdateEngine() {
		float forwardSpeed = Vector2.Dot(transform.up, rigidbody.velocity);
		if (accelerationInput > 0 && rigidbody.velocity.sqrMagnitude >= maxSpeed * maxSpeed) return;
		if (accelerationInput > 0 && forwardSpeed >= maxSpeed) return;
		if (accelerationInput < 0 && forwardSpeed <= -maxSpeed) return;

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
