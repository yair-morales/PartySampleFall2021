using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicVehicleInputHandler : MonoBehaviour {
	
	public BasicVehicleMotor motor;

	private void Awake() {
		if (motor == null) motor = GetComponent<BasicVehicleMotor>();
	}

	private void Update() {
		motor.accelerationInput = Input.GetAxis("Vertical");
		motor.steeringInput = Input.GetAxis("Horizontal");
	}
}