using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATM : MonoBehaviour
{

    public int speedThreshold;
    public float explosionForce;
    public ParticleSystem explosionEffect;
    public GameObject moneyPrefab;
    public int moneyAmount;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<CollisionManager>())
        {
            //We know its player
            CollisionManager CM = collision.transform.GetComponent<CollisionManager>();
            float speedHitAt = CM.transform.GetComponent<Rigidbody2D>().velocity.magnitude;
            if (speedHitAt >= speedThreshold)
            {
                //Instantiate(explosionEffect, transform.position, explosionEffect.transform.rotation);
                for(int i=0; i < moneyAmount; i++)
                {
                    GameObject money = Instantiate(moneyPrefab, transform.position + new Vector3(Random.RandomRange(-3,3)+2, Random.RandomRange(-3, 3)+2, 0), moneyPrefab.transform.rotation);

                }
                Debug.Log("oof");
                Destroy(this.gameObject);
            }
        }
    }
}
