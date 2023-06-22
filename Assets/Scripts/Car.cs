using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed;
    public GameObject explosionVFX;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        //rb.velocity = new Vector2(moveSpeed, 0f);
        rb.AddForce(new Vector2(moveSpeed, 0f), ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Collider")
        {
            GameObject explosion = Instantiate(explosionVFX, transform.position, transform.rotation);
            Destroy(explosion, .75f);
            Destroy(gameObject);
        }
    }
}
