using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy stats")]
    
    [SerializeField] float moveSpeed = 1f;

    [Header("Shooting")]
   
    Rigidbody2D myRigidBody;
    public Animator animator;

    Transform player;
    #region Public
    public int maxHealth = 100;
    public int currentHealth;
    public int attackDamage = 40;
    #endregion

    #region Private
    #endregion

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
    }

   
    void Update()
    {
        Moving();
    }

    

    private void Moving()
    {
        if (IsFacingLeft())
        {
            myRigidBody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
        }
    }



    bool IsFacingLeft()
    {
        return transform.localScale.x < 0;
    }

    private void OnTriggerExit2D(Collider2D collison)
    {
        transform.localScale = new Vector2((Mathf.Sign(myRigidBody.velocity.x)), 1f);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        
        Debug.Log("Enemy died!");

        animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        myRigidBody.velocity = Vector2.zero;
        myRigidBody.isKinematic = true;
        StartCoroutine(DestroyingObject());
        this.enabled = false;
        
    }

    IEnumerator DestroyingObject()
    {
        yield return new WaitForSeconds(3);     
        Destroy(gameObject);
    }

}
