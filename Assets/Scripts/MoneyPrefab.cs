using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPrefab : MonoBehaviour
{
    public int cashAmount;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<CollisionManager>())
        {
            Debug.Log(cashAmount);
            Destroy(this.gameObject);
        }
    }
}
