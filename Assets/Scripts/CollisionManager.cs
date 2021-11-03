using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public Rigidbody2D rb;
    public ParticleSystem whamEffect;

    //Displays Paritcle Effect when hitting something
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            whamEffect.transform.position = collision.contacts[0].point;
            whamEffect.transform.forward = transform.forward;
            whamEffect.transform.localScale = new Vector3(rb.velocity.magnitude/5,rb.velocity.magnitude/5,rb.velocity.magnitude/5);
            foreach (Transform child in whamEffect.transform)
            {
                child.GetComponent<ParticleSystem>().Emit(1);
            }
        }
    }
}
