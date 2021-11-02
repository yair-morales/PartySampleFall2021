using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoliceCar : MonoBehaviour {
    public BasicVehicleMotor motor;
    public Transform frame;
    public Transform[] wheels;
    public Transform player;
    public float moveSpeed = 5;
    public float chaseDist = 5;
    public float stopDist = 8;

    private float dist;


    private void Awake() {
        if (motor == null) motor = GetComponent<BasicVehicleMotor>();
    }

    private void Update() {
        Vector3 displacement = player.position - frame.position;
        dist = displacement.magnitude;
        if (dist > stopDist)
        {
            motor.accelerationInput = 0;
        } else if (chaseDist < dist && dist <= stopDist)
        {
            motor.accelerationInput = 0.5f;
        } else
        {
            motor.accelerationInput = 1;
        }
        Vector3 direction = player.position - transform.position;
		direction.Normalize();
		float strInput = Vector3.Dot(direction, transform.right);
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //motor.rigidbody.rotation = strInput;
		//float normAngle = angle / 180;
        //direction.Normalize();
		//frame.LookAt(player);

        //motor.accelerationInput = ;
        motor.steeringInput = strInput;
            
        if (wheels != null)
        {
            foreach (var wheel in wheels)
            {
                Vector3 rot = frame.eulerAngles;
                float zRot = Mathf.LerpUnclamped(0, 45, strInput);
                rot.z += zRot;
                wheel.eulerAngles = rot;
            }
        }
    }
    
}