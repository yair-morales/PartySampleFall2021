using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthScript : MonoBehaviour {
    
    public int hp;
    public Action<Vector3> onDeathAction;
    public bool dead; 
    public int maxHP;

    public void OnDamageTaken(int dmg, Vector3 dir) {
        if (dmg < 0 || dead) return;
        hp -= dmg;
        if (hp <= 0) {
            onDeathAction?.Invoke(dir);
            dead = true;
        }
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
