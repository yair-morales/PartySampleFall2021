using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int HP;
    public Action onDeathAction;
    public bool dead; 
    public int maxHP;

    public void OnDamageTaken(int dmg, Vector3 dir)
    {
        if (dmg < 0 || dead) return;
        HP -= dmg;
        if (HP <= 0)
        { 
            onDeath?.Invoke();
            dead = true;
        }
    }

    public void OnHealthRestore(int heal, Vector3 dir)
    {
        if (HP >= maxHP) return;
        if (HP + heal > maxHP)
        {
            HP = maxHP;
        }
        else
        {
            HP += heal;
        }
    }
}
