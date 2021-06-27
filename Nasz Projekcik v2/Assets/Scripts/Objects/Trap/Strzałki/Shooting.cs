using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] public float laserSpeed = .1f;
    [SerializeField] public float Damage;
    [SerializeField] int direction_x;
    [SerializeField] int direction_y;
    Rigidbody2D rb;
    
   
    void Start()
    {
      
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(direction_x * laserSpeed , direction_y * laserSpeed);
       

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<KontrolerGry>().TakingDamage(Damage);

            Destroy(gameObject);
        }
         else
        Destroy(gameObject);
    }

}
