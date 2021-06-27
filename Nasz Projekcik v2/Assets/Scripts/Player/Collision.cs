using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    // skrypt kolizji z tego co radek wysylal 'Celeste's Movement | Mix and Jam'
    // caly skrypt sluzy do sprawdzania kolizji ze scianami i podlaga, niezaleznie od capsule i box collidera istniejacych na playerze

    // jak chcesz sprawdzic jak dziala to odpal gre z widocznym Player'em w inspektorze, podejdz do sciany i zobacz jak sie zmieniaja wartosci


    // cala ta sekcja to parametry do mozliwe do zmiany w inspektorze
    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask WallLayer;

    [Space]

    public bool onSlidingWall;
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public bool underSomething;
    public bool canGrab = false;
    public bool isGrabing;
    public float gravityStore;
    public float gravityStoreWalljump = 1;
    public float wallJumpTime = 1f;
    public float wallJumpCounter;
    public int wallSide = 0;
    

    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset, topOffset;
  //  public Vector2 rightOffset2, leftOffset2;

    void Update()
    {
       
        CheckingCollision();
    }

    private void CheckingCollision()
    {
        // OverlapCircle sprawdza, czy jakis Collider znajduje siê w obszarze kola. Tutaj pobiera parametry (srodek_kola(Vector2), 
        //                                                                                            promien_kola(float), 
        //                                                                                            filtr_zeby_sprawdzic_obiekty_tylko_na_okreœlonych_warstwach(LayerMask))
       // && Physics2D.OverlapCircle((Vector2)transform.position + rightOffset2, collisionRadius, groundLayer)

        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        

        underSomething = Physics2D.OverlapCircle((Vector2)transform.position + topOffset, collisionRadius, groundLayer);

        onSlidingWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, WallLayer)
           || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, WallLayer);

        if (onRightWall)
        {
            wallSide = 1;
        }
        if (onLeftWall)
        {
            wallSide = -1;
        }
        if (onGround)
        {
            wallSide = 0;
        }
    }


    // rysuje te czerwone kola na playerze, ktore sluza do wykrywania kolizji
    void OnDrawGizmos()
    {
       

        Gizmos.color = Color.red;


        Gizmos.DrawWireSphere((Vector2)transform.position + topOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
       // Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset2, collisionRadius);
       // Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset2, collisionRadius);
    }




}
