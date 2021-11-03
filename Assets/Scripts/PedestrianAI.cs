using System;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Rigidbody2D))]
public class PedestrianAI : MonoBehaviour {

	public float decisionInterval = .5f;
	public float runawaySpeed = 5f;
	public float calmDistance = 15f;

	public Rigidbody2D rigidbody;
	public SimpleWalk walk;
	public HealthScript health;

	public bool dead;
	public bool runaway;
	public Transform from;
	public Vector3 direction;
	public float lastDecisionTime;

	public ParticleSystem bloodPlayer;
	public ParticleSystem deathPlayer;
	public SpriteRenderer sprite;
	public Color originalColor;
	public Color panicColor;
	public GameObject corpse;
	
	private void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
		walk = GetComponent<SimpleWalk>();
		health = GetComponent<HealthScript>();
		if (health) {
			health.onDmgAction += OnHit;
			health.onDeathAction += (dir, tf) => {
				rigidbody.velocity = dir * 2;
				deathPlayer?.Play();
				if (corpse) Instantiate(corpse, transform.position, Quaternion.identity);
			};
		}

		if (sprite) {
			originalColor = sprite.color;
		}
	}

	public void Update() {
		if (dead || !runaway || !from) return;
		
		var disp = transform.position - from.position;
		var dist = disp.magnitude;
		if (dist >= calmDistance) {
			runaway = false;
			from = null;
			if (walk) walk.enabled = true;
			if (sprite) sprite.color = originalColor;
		}
		var time = Time.timeSinceLevelLoad;
		if (time - lastDecisionTime >= decisionInterval) {
			lastDecisionTime = time;
			direction = disp / dist;
		}

		rigidbody.velocity = direction * runawaySpeed;
	}

	public void OnHit(Vector3 dir, Transform from) {
		Frighten(from);
		bloodPlayer.Play();
	}

	public void Frighten(Transform from) {
		runaway = true;
		this.from = from;
		walk.enabled = false;
		direction = (transform.position - from.position).normalized;
		lastDecisionTime = Time.timeSinceLevelLoad;
		if (sprite) sprite.color = panicColor;
	}
}
