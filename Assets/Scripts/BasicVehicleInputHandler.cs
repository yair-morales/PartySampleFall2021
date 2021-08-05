using UnityEngine;

public class BasicVehicleInputHandler : MonoBehaviour {
	
	public BasicVehicleMotor motor;
	public Transform frame;
	public Transform[] wheels;

	private void Awake() {
		if (motor == null) motor = GetComponent<BasicVehicleMotor>();
	}

	private void Update() {
		float verticalInput = Input.GetAxis("Vertical");
		float horizontalInput = Input.GetAxis("Horizontal");

		motor.accelerationInput = verticalInput;
		motor.steeringInput = horizontalInput;

		float wheelRotation = -horizontalInput;

		if (wheels != null) {
			foreach (var wheel in wheels) {
				Vector3 rot = frame.eulerAngles;
				float zRot = Mathf.LerpUnclamped(0, 45, wheelRotation);
				rot.z += zRot;
				wheel.eulerAngles = rot;
			}
		}
	}
}