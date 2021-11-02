using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthScript : MonoBehaviour {
    
    public int hp;
    public Action<Vector3, bool> onDeathAction;
    public bool dead; 
    public int maxHP;

    public bool OnDamageTaken(int dmg, Vector2 dir, bool fromPlayer = true) {
        if (dmg < 0 || dead) return false;
        hp -= dmg;
        if (hp <= 0) {
            onDeathAction?.Invoke(dir, fromPlayer);
            dead = true;
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
