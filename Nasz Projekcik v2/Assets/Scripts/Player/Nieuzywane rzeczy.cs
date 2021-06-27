using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nieuzywanerzeczy : MonoBehaviour
{
    /*
                       //////////////////////////// Stary sposób na state'y postaci (animacje)

       // private enum State { Idle, Running, Jumping, Falling, Hurting, Dying, Crouching , WallSliding }
     // private State state = State.Idle; 
    -----------------------------------------------------------------------------------------------------------------
     //if (isHurt == false)
           // betterJump();
    -----------------------------------------------------------------------------------------------------------------
     /*if (isHurt == false)
                {


                }
    -----------------------------------------------------------------------------------------------------------------
    //myAnimator.SetInteger("State", (int)state);
            //velocityState(); /////////////////// Wywolanie animacji w animatorze
    -----------------------------------------------------------------------------------------------------------------
               // state = State.WallSliding;
            //StartCoroutine(gravityScaling());
    -----------------------------------------------------------------------------------------------------------------
              
            // StartCoroutine(gravityScaling());
    -----------------------------------------------------------------------------------------------------------------
           // if ((transform.localScale.x == -1f /*&& Input.GetAxisRaw("Horizontal") < 0/))
      //  if ((transform.localScale.x == 1f )) ////////////////////////// Stare LocalScale
    -----------------------------------------------------------------------------------------------------------------
         // col.canGrab = true; ////////////////////////////////////// Do grabowania sie to bylo 

    -----------------------------------------------------------------------------------------------------------------
     //PIERWSZY SPOSOB
        //Vector2 jumpVelocityToAdd = new Vector2(myRigidbody.velocity.x, jumpSpeed);
        /*myRigidbody.velocity += jumpVelocityToAdd;
        //DRUGI SPOSOB
        //Mozna to zapisac w jednej linijce jakby ktos nie rozumial

    -----------------------------------------------------------------------------------------------------------------
     /*private void betterJump()
    {
        if (!DisableBetterJump)
        {

            if (myRigidbody.velocity.y < 0)
            {
                myRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (myRigidbody.velocity.y > 0 && weakBetterJump)
            {
                myRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    } /////////////////////////////////////////////////////////////Stary BetterJump
    -----------------------------------------------------------------------------------------------------------------
     /* private void velocityState()
     {
         if (state == State.WallSliding)
         {

             if (!col.canGrab)
             {
                 state = State.Jumping;
             }
         }

         else if (state == State.Jumping)
         {
             if(col.onWall)
             {
                 state = State.WallSliding;
             }

             if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Foreground")) && myRigidbody.velocity.y < .1f)
             {
                 state = State.Idle;
             }
         }
         else if (Math.Abs(myRigidbody.velocity.x) > 1.5f && state != State.Crouching)
         {
             state = State.Running;
         }
         else if (state == State.Hurting)
         {
             if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Foreground")) && Math.Abs(myRigidbody.velocity.y) < .1f)
             {
                 state = State.Idle;
             }
         }
         else if (state != State.Crouching)
         {
             state = State.Idle;
         }
         else if (state == State.Crouching && !isCrouching && !col.underSomething)
         {
             state = State.Idle;
         }


     }////////////////////////////////////////////////////////////////////////////////Stary sposób na State'y
    -----------------------------------------------------------------------------------------------------------------
     /*IEnumerator gravityScaling()
    {
        if (col.isGrabing && col.onWall)
        {
            myRigidbody.gravityScale = 0;
        }
        if (Walljumped || jumped)
            {

                myRigidbody.gravityScale = 2;
                if (col.onGround)
                {
                    myRigidbody.gravityScale = col.gravityStore;
                }
                else
                {
                    yield return new WaitForSeconds(timeToNormalGravity);
                    myRigidbody.gravityScale = col.gravityStore;
                }
            }
        
        else if(!col.isGrabing && !Walljumped)
        {
            myRigidbody.gravityScale = col.gravityStore;
        }

    } ////////////////////////////////////////////////Skalowanie grawitacji 
    ---------------------------------------------------------------------------------------------------------------
    */
     
   

}
