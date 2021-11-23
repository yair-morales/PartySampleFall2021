using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyGrab : MonoBehaviour
{
    public int bal;

    public bool PayMoney(int amt)
    {
        if (amt < 0) return false;
        if (bal < amt) return false;
        bal -= amt;
        return true;
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out MoneyPrefab money))
        {
            bal += money.cashAmount;
            Destroy(other.gameObject);
        }
    }
}
