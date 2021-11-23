using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoliceCar : MonoBehaviour {
    
    public ParticleSystem dustPlayer;
    public float dustThreshold = .9f;
    public BasicVehicleMotor motor;
    public Transform frame;
    public Transform[] wheels;
    public Transform player;
    public float chaseDist = 5;
    public float stopDist = 8;
    public float detectDist = 15;

    public float dmgMinInterval = .5f;
    public float minSpeedToDmg = 1f;
    public int minSmashDmg = 25;
    public int maxSmashDmg = 120;

    private float dist;
    private bool dustMark;
    private Dictionary<int, float> _dmgInfo = new Dictionary<int, float>();
    private List<int> _tempList = new List<int>();

	private CrimeManager crimeManager;
	private bool alerted;

    private void Awake() {
        if (motor == null) motor = GetComponent<BasicVehicleMotor>();
		crimeManager = GetComponent<CrimeManager>();
		if (crimeManager) crimeManager.onCrimeHappen += UpdateCrime;
    }

    private void Update() {
		if (!alerted) {
			motor.accelerationInput = 0;
			return;
		}
		Vector3 direction = player.position - transform.position;
		direction.Normalize();
		float strInput = Vector3.Dot(direction, transform.right);
        motor.steeringInput = strInput;

        Vector3 displacement = player.position - frame.position;
        dist = displacement.magnitude;
        if (dist > stopDist)
       	{
			alerted = false;
            motor.accelerationInput = 0;
            motor.boostInput = 0;
        } else if (chaseDist < dist && dist <= stopDist)
        {
            motor.accelerationInput = 1f;
            motor.boostInput = 0;
        } else
        {
            motor.accelerationInput = 1;
            motor.boostInput = 1;
        }
        
        
        if (Mathf.Abs(strInput) >= dustThreshold) MakeDust();
        else StopDust();
            
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
        
        UpdateDmgInfo();
    }

	private void UpdateCrime(Vector3 position, Vector3 dir) {
		Vector3 direction = player.position - transform.position;
		direction.Normalize();
		float strInput = Vector3.Dot(direction, transform.right);

		if (strInput > 0 && direction.magnitude < detectDist) {
			//crimeManager.onCrimeHappen.Invoke(player.position, direction);
			alerted = true;
		}
	}
    
    private void UpdateDmgInfo() {
		
        _tempList.Clear();
		
        foreach (var info in _dmgInfo) {
            if ((Time.timeSinceLevelLoad - info.Value) >= dmgMinInterval) _tempList.Add(info.Key);
        }

        foreach (var id in _tempList) _dmgInfo.Remove(id);
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
    
    private void Hit(Collision2D other) {
        if (!other.collider.TryGetComponent<HealthScript>(out var health)) return;
		
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
    }
    
    private void OnCollisionEnter2D(Collision2D other) => Hit(other);
	
    private void OnCollisionStay2D(Collision2D other) => Hit(other);
}