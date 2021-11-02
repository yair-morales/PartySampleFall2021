using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public float bulletSpeed = 20f;
    public Rigidbody2D rb;
    //public Vector2 direction = gunner.rotation;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.up  * bulletSpeed; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
