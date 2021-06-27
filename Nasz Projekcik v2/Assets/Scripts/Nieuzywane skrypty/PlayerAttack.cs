using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{

    public Animator animAttack;

    public Transform AttackPoint;
    public LayerMask EnemyLayers;

    public float attackRange = 0.5f;
    public int attackDamage = 40;

    public float attackRate = 2f;
    float nextAttacktime = 0f;


    public void AttackInput(InputAction.CallbackContext context)
    {
        if (Time.time >= nextAttacktime)
        {
            Attack();
            nextAttacktime = Time.time + 1f / attackRate;
        }
    }




    private void Attack()
    {
        animAttack.SetTrigger("Attack");

        // wykrywanie przeciwnika w zasiêgu ataku
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, EnemyLayers);

        foreach(Collider2D Enemy in hitEnemies)
        {
            Enemy.GetComponent<EnemyScript>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
    }
}
