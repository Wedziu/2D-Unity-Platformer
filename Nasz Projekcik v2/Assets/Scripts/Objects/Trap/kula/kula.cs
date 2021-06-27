using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kula : MonoBehaviour
{
    [SerializeField] public float Damage;
    [SerializeField] public float HurtCoolDown;
    [SerializeField] public float hazardCooldown;
    private Player player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player.HazardDamage(Damage, HurtCoolDown, hazardCooldown);
        }
    }


   
}
