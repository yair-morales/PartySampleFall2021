using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ATM : MonoBehaviour {
    public GameObject moneyPrefab;
    public int moneyAmount;

    public HealthScript health;
    // Start is called before the first frame update
    private void Awake() {
        health = GetComponent<HealthScript>();
        health.onDeathAction += OnHit;
    }

    private void OnHit(Vector3 dir, Transform from) {
        for(int i=0; i < moneyAmount; i++) Instantiate(moneyPrefab, transform.position + new Vector3(Random.Range(-3,3)+2, Random.Range(-3, 3)+2, 0), moneyPrefab.transform.rotation);
    }
}
