using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthScript : MonoBehaviour {
    
    public int hp;
    public event Action<Vector3, Transform> onDmgAction;
    public event Action<Vector3, Transform> onDeathAction;
    public bool dead;
    public int maxHP;
    public bool destroyOnDeath;
    public float destroyDelay;
    public float destroyRecord;

    private void Awake() {
        hp = maxHP;
    }

    private void Update() {
        if (dead && destroyOnDeath && Time.timeSinceLevelLoad - destroyRecord >= destroyDelay) Destroy(gameObject);
    }

    public bool OnDamageTaken(int dmg, Vector2 dir, Transform from) {
        if (dmg < 0 || dead) return false;
        hp -= dmg;
        onDmgAction?.Invoke(dir, from);
        if (hp <= 0) {
            onDeathAction?.Invoke(dir, from);
            dead = true;
            destroyRecord = Time.timeSinceLevelLoad;
            return true;
        }

        return false;
    }

    public void OnHealthRestore(int heal) {
        if (hp >= maxHP) return;
        if (hp + heal > maxHP) {
            hp = maxHP;
        } else {
            hp += heal;
        }
    }
}
