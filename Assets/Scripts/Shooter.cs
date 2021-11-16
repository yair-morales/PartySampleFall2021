using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public KeyCode fire;

    public Transform cursor;

    private Vector3 mousePosition;

    // Update is called once per frame
    void Update()
    {

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        if (cursor) cursor.position = mousePosition;
        firePoint.up = (mousePosition - firePoint.position).normalized;

        if (Input.GetKey(fire)){
            Shoot();
        }
    }

    void Shoot(){
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
