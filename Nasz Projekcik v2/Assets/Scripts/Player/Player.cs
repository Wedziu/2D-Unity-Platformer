using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Particle Effects")]
    [SerializeField] ParticleSystem dust;
    public ParticleSystem impactEffect;
    public ParticleSystem jumpingParticles;
    private ParticleSystem.EmissionModule jumpingParticlesEmission;
    private ParticleSystem.EmissionModule dustEmission;
    

    [Header("Movement")]
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;

    [Header("Better Jump")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("WallJump")]
    [SerializeField] float WallJumpSpeed = 2;
    public float timeToNormalGravity = 3f;


    [Header("Damage Kick")]
    [SerializeField] Vector2 damageKickLeft = new Vector2(-20f, 20f);
    [SerializeField] Vector2 damageKickRight = new Vector2(20f, 20f);


    [Header("Interaction")]
    [SerializeField] Vector2 interactBoxSize;

    /// <summary>
    /// Miejsce na zmienne do biegu
    
    /// </summary>


    public bool openMenu;
    public bool isAttacking;
    public bool isFirstAttack;
    public bool isAlive = true;
    public bool isHurt = false;
    public bool DisableBetterJump = false;
    public bool DisableNewBetterJump = false;
    public bool isCrouching;
    public bool isUsingKeyboard;
    public bool Walljumped;
    public bool isDashing;
    public bool jumped = false;
    public bool skoczylem;
    public bool hazardHurt = false;
   
    private bool wasOnGround;
    private bool isFacingRight = true;
    private float onTouchDamage = 10;

    public float lastHurtTime;
    public float moveInput;
    public float dashTime;
    public float dashSpeed;
    public float dashCoolDown;
    [SerializeField]public int dashCost = 50; 
    private float dashTimeLeft;
    private float lastDash = -100f;
    float hurtCooldown = 0f;
    float hazardCooldown = 0f;


    private int currentScene;
 
    public float hangTime = 0.05f;
    public float hangCounter;

    public float grabTime = 0.5f;
    public float grabCounter;
    public bool grabbed;

    public float wallJumpTime = 1f;
    public float wallJumpCounter;

    Rigidbody2D myRigidbody;
    CapsuleCollider2D feetCollider;
    Animator myAnimator;
    BoxCollider2D headCollider;
    Collision col;

    public GameObject interactableIcon;

    //Animation States
    private string currentState;
    const string Idling = "Idling";
    const string Running = "Running";
    const string Jumping = "Jumping";
    const string Crouch = "Crouch";
    const string WallSliding = "WallSliding";
   // const string Hurting = "Hurting";
    const string PlayerAttack1 = "PlayerAttack1";
    const string PlayerAttack2 = "PlayerAttack2";
    const string Dying = "Dying";
    const string Dash = "Dash";


    //Animation Hurt States
 
    const string HurtIdling = "HurtIdling";
    const string HurtRunning = "HurtRunning";
    const string HurtJumping = "HurtJumping";
    const string HurtCrouch = "HurtCrouch";
    const string HurtWallSliding = "HurtWallSliding";
    const string HurtPlayerAttack1 = "HurtPlayerAttack1";
    const string HurtPlayerAttack2 = "HurtPlayerAttack2";


 

    public PlayerCombatController playerCombatController;

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        myAnimator.Play(newState);

        currentState = newState;
    }


    private void GettingComponents()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        feetCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
        headCollider = GetComponent<BoxCollider2D>();
        col = GetComponent<Collision>();
        playerCombatController = GameObject.Find("Player").GetComponent<PlayerCombatController>();

    }
    void Start()
    {
        GettingComponents();
        interactableIcon.SetActive(false);
        getCurrentDevice();
        headCollider.size = new Vector2(0.60f, 0.85f);
        headCollider.offset = new Vector2(0.05f, 0.16f);
        currentScene = SceneManager.GetActiveScene().buildIndex;
        dustEmission = dust.emission;
        jumpingParticlesEmission = jumpingParticles.emission;
        isHurt = false;
        myRigidbody.velocity *= Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        deathlayertouch();
        Damage(onTouchDamage);
           if(!isAlive)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x * 0.95f, myRigidbody.velocity.y * 0.95f);
        }
        if (col.onWall && myRigidbody.velocity.y<0f)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y * 0.7f);
        }

        if (col.wallJumpCounter <= 0)
        {
            if (!hazardHurt)
            {
                Move();
                //CheckWallGrabing();
                Walljump();
            }

        }
        else
        {
            col.wallJumpCounter -= Time.deltaTime;
        }
        NewWallGrabing();

    }
    void Update()
    {
        
        if (Time.timeScale == 0)
        {
            openMenu = true;
        }
        else
        {
            openMenu = false;
        }

        if (!isAlive) {
            ChangeAnimationState(Dying); return; 
        }

        if (!openMenu)
        {
            isAttacking = playerCombatController.isAttacking;
            isFirstAttack = playerCombatController.isFirstAttack;

            StateMachine();


            if (!hazardHurt)
            {
                FlipSprite();
            }
            
            CheckCrouching();
            CheckHurt();
            CheckHazard();
            CheckDash();
            CoyoteTimer();
            WallGrabTimer();
            if (isAlive)
            {
                FootEmissionParticles();
                JumpingEmissionParticles();
            }
            wallJumpTimer();

            if (!wasOnGround && col.onGround)
            {
                impactEffect.gameObject.SetActive(true);
                impactEffect.Stop();
                impactEffect.transform.position = dust.transform.position;
                impactEffect.Play();
                skoczylem = false;
            }
            wasOnGround = col.onGround;
        }
        
        
    }
    void FootEmissionParticles()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 && col.onGround)
        {
            dustEmission.rateOverTime = 35f;

        }
        else
        {
            dustEmission.rateOverTime = 0f;
        }
    }
    void JumpingEmissionParticles()
    {
        if (!col.onGround && !col.onWall)
        {
            jumpingParticlesEmission.rateOverTime = 100;
        }
        else
        {
            jumpingParticlesEmission.rateOverTime = 0;
        }
    }
    private void CheckCrouching()
    {
        if (!isCrouching && !col.underSomething)
        {
            headCollider.size = new Vector2(0.60f, 0.85f);
            headCollider.offset = new Vector2(0.05f, 0.16f);
        }
    }
    public void CoyoteTimer()
    {
        if (col.onGround)
        {
            hangCounter = hangTime;
            skoczylem = false;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }
        
          
    }
    public void WallGrabTimer()
    {

        if (!col.onWall)
        {
            grabbed = false;
            grabCounter = grabTime;
        }
            if (col.onLeftWall)
            {

                if (Input.GetAxisRaw("Horizontal") == 1)
                    grabCounter -= Time.deltaTime;
            }
            else if (col.onRightWall)
            {
                if (Input.GetAxisRaw("Horizontal") == -1)
                    grabCounter -= Time.deltaTime;
            }
        }
    
    
    public void wallJumpTimer()
    {
        if(Walljumped)
        {
            wallJumpCounter = wallJumpTime;
        }    
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }
    }


    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
        FindObjectOfType<KontrolerGry>().ManageStaminaOnDodge(dashCost);
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                
                isHurt = true;
                if(isFacingRight)
                myRigidbody.velocity = new Vector2(dashSpeed, myRigidbody.velocity.y);
                if(!isFacingRight)
                myRigidbody.velocity = new Vector2(-dashSpeed, myRigidbody.velocity.y);
                dashTimeLeft -= Time.deltaTime;
                
            }

            if(dashTimeLeft<=0 || col.onWall)
            {
                isDashing = false;
                isHurt = false;
            }
        }
    }
    private void Walljump()
    {
        if (grabCounter>0f && !col.onSlidingWall)
        {
            DisableBetterJump = true;
           // DisableNewBetterJump = true;
             
            //myRigidbody.velocity = Vector2.zero;

            if (Walljumped)
            {
                col.wallJumpCounter = col.wallJumpTime;
                if (col.onLeftWall )
                {

                    // myRigidbody.velocity = new Vector2(((WallJumpSpeed * runSpeed)/2), jumpSpeed);
                    
                    myRigidbody.velocity = new Vector2(Mathf.MoveTowards(myRigidbody.velocity.x+7f,7f, 8f), Mathf.MoveTowards(jumpSpeed, 30f, 2f));
                }
                if (col.onRightWall )
                {
                    //myRigidbody.velocity = new Vector2(((-WallJumpSpeed * runSpeed)/2), jumpSpeed);
                    myRigidbody.velocity = new Vector2(Mathf.MoveTowards(myRigidbody.velocity.x - 7f, -7f, 8f), Mathf.MoveTowards(jumpSpeed, 30f, 2f));
                }
               
                
                col.canGrab = false;

                Walljumped = false;
                
            }
        }
        else
        {
          
            if (col.onGround)
            {
               
                DisableBetterJump = false;
                DisableNewBetterJump = false;
            }
           // if (!col.onWall)
           // {
            //    col.canGrab = false;
            //}
                 
        }
    }

   /* private void CheckWallGrabing()
    {
     
         if (col.onLeftWall && !col.onGround)
        {
                   col.canGrab = true;
            
        }
        else if (col.onRightWall && !col.onGround)
        {
          col.canGrab = true;

        }
    }*/

    public void getCurrentDevice()
    {
        
        InputSystem.onActionChange += (obj, change) =>
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction)obj;
                var lastControl = inputAction.activeControl;
                var lastDevice = lastControl.device;

                Debug.Log($"device: {lastDevice.displayName}");

                if (lastDevice.name == "Keyboard" || lastDevice.name == "Mouse")
                {
                    isUsingKeyboard = true;
                }
                else 
                {
                    isUsingKeyboard = false;
                }
            }
        };

    }

    private void Move()
    {
        if (col.onGround) ///////////////////////////////TO JEST OPOZNIENIE "SLIZGANIE SIE" PO ZIEMII
        {
            grabbed = false;
            if (Input.GetAxisRaw("Horizontal") == -1)
            {
                //Vector2 playerVelocity = new Vector2((moveInput * runSpeed) / 2f, myRigidbody.velocity.y);

                // Vector2 playerVelocity = new Vector2((-runSpeed / 2f), myRigidbody.velocity.y);
                //myRigidbody.velocity = playerVelocity;
                if (myRigidbody.velocity.x > 0)
                {
                    myRigidbody.velocity = new Vector2(Mathf.MoveTowards(myRigidbody.velocity.x, -6f * 3f, 1.5f), myRigidbody.velocity.y);
                }
                else
                {
                    myRigidbody.velocity = new Vector2(Mathf.MoveTowards(myRigidbody.velocity.x, -6f, 1f), myRigidbody.velocity.y);
                }
            }
            else if (Input.GetAxisRaw("Horizontal") == 1)
            {

                //  Vector2 playerVelocity = new Vector2(runSpeed / 2f, myRigidbody.velocity.y);
                //  myRigidbody.velocity = playerVelocity;

                if (myRigidbody.velocity.x < 0)
                {
                    myRigidbody.velocity = new Vector2(Mathf.MoveTowards(myRigidbody.velocity.x, 6f * 3f, 1.5f), myRigidbody.velocity.y);
                }
                else
                {
                    myRigidbody.velocity = new Vector2(Mathf.MoveTowards(myRigidbody.velocity.x, 6f, 1f), myRigidbody.velocity.y);
                }
            }
            else if (Input.GetAxisRaw("Horizontal") == 0)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x * 0.7f, myRigidbody.velocity.y);

            }

        }
        else if (!col.onGround && !grabbed && !col.onWall) ///////////////////////////TO JEST GLIDING PODCZAS NORMALNEGO SKOKU
        {
            if (Input.GetAxisRaw("Horizontal") == -1)
            {
                //myRigidbody.velocity = new Vector2((-runSpeed) / 1.25f, myRigidbody.velocity.y);
                if (myRigidbody.velocity.x > 0)
                {
                    myRigidbody.velocity = new Vector2(Mathf.MoveTowards(myRigidbody.velocity.x, -runSpeed * 8f, 2f), myRigidbody.velocity.y);
                    Debug.Log("Mam predkosc wieksza niz 0");
                }
                else
                {
                    Debug.Log("mam predkosc mniejsza niz 0");
                    myRigidbody.velocity = new Vector2(Mathf.MoveTowards(myRigidbody.velocity.x, -runSpeed / 1.25f, .6f), myRigidbody.velocity.y);
                }
            }

            else if (Input.GetAxisRaw("Horizontal") == 1)
            {
                //myRigidbody.velocity = new Vector2((runSpeed) / 1.25f, myRigidbody.velocity.y);
                if (myRigidbody.velocity.x < 0)
                {
                    myRigidbody.velocity = new Vector2(Mathf.MoveTowards(myRigidbody.velocity.x, runSpeed * 8f, 2f), myRigidbody.velocity.y);
                }
                else
                {
                    myRigidbody.velocity = new Vector2(Mathf.MoveTowards(myRigidbody.velocity.x, runSpeed / 1.25f, .6f), myRigidbody.velocity.y);
                }

            }
            else
            {
                // myRigidbody.velocity = new Vector2(myRigidbody.velocity.x * 0.95f, myRigidbody.velocity.y);
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x * 0.95f, myRigidbody.velocity.y);
            }
        }
        else if (skoczylem && !grabbed) ///////////////////////////TO JEST ZE SCIANY SKOK I "GLIDING"
        {
            
            if (Input.GetAxisRaw("Horizontal") == -1)
            {
                //myRigidbody.velocity = new Vector2((-runSpeed) / 1.25f, myRigidbody.velocity.y*.95f);

                if (myRigidbody.velocity.x > 0)
                {
                    myRigidbody.velocity = new Vector2(Mathf.MoveTowards(myRigidbody.velocity.x, -runSpeed * 12f, 3f), myRigidbody.velocity.y);
                    Debug.Log("Mam predkosc wieksza niz 0");
                }
                else
                {
                    Debug.Log("mam predkosc mniejsza niz 0");
                    myRigidbody.velocity = new Vector2(Mathf.MoveTowards(myRigidbody.velocity.x, -runSpeed, .8f), myRigidbody.velocity.y);
                }
            }

            else if (Input.GetAxisRaw("Horizontal") == 1)
            {
                //myRigidbody.velocity = new Vector2((runSpeed) / 1.25f, myRigidbody.velocity.y*.95f);
                if (myRigidbody.velocity.x < 0)
                {
                    myRigidbody.velocity = new Vector2(Mathf.MoveTowards(myRigidbody.velocity.x, runSpeed * 12f, 3f), myRigidbody.velocity.y);
                }
                else
                {
                    myRigidbody.velocity = new Vector2(Mathf.MoveTowards(myRigidbody.velocity.x, runSpeed, .8f), myRigidbody.velocity.y);
                }
            }
            else
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x * 0.98f, myRigidbody.velocity.y);

            }
        }
        else if(grabCounter<0f && col.onWall && !col.onGround)
        {
           if(Input.GetAxisRaw("Horizontal") == 1)
            {
                myRigidbody.velocity = new Vector2(Mathf.MoveTowards(myRigidbody.velocity.x, 6f, .8f), myRigidbody.velocity.y);
            }
            else if(Input.GetAxisRaw("Horizontal") == -1)
            {
                myRigidbody.velocity = new Vector2(Mathf.MoveTowards(myRigidbody.velocity.x, -6f, .8f), myRigidbody.velocity.y);
            }
        }
    }
    public void onWallGrabInput(InputAction.CallbackContext context)
    { 
       /* if (context.performed)
        {
            
            Debug.Log("kliknalem");

            if (col.canGrab)
            {

                col.isGrabing = true;

            } 
        }
        if (context.canceled)
        {
            Debug.Log("Puscilem shifta");

            col.isGrabing = false;
            
            
        }*/
    }

    public void onMovementInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().x;
        
    }
    public void NewWallGrabing()
    {
        if (col.onWall)
        {
            grabbed = true;
            if (col.onLeftWall)
            {
                if (Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Horizontal") == 0)
                {
                    col.isGrabing = true;
                }
                if(Input.GetAxisRaw("Horizontal") == 1)
                {
                    if (grabCounter > 0f)
                    {
                        col.isGrabing = true;
                        
                    }
                    else
                    {
                        col.isGrabing = false;
                    }
                    
                }
            }
            if (col.onRightWall)
            {
                if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == 0)
                {
                    col.isGrabing = true;
                }
                else if(Input.GetAxisRaw("Horizontal") == -1)
                {
                    if (grabCounter > 0f )
                    {
                        col.isGrabing = true;
                        
                    }
                    else
                    {
                        col.isGrabing = false;
                    }
                    
                }
            }

        }
    }

    public void onJumpingInput(InputAction.CallbackContext context)
    {
        if (isAlive && !hazardHurt)
        {
          
            if (context.performed )
            {
                
                if (grabCounter>0 && !col.onGround)
                {
                    Walljumped = true;
                    skoczylem = true;
                    
                }
                if (hangCounter > 0f)
                {

                    normalJump();
                    
                }
                
             //   if (!col.canGrab && !col.onWall)
                //Walljumped = false;
            }

            if (context.canceled && myRigidbody.velocity.y > 0)
            { 
                
                if (!DisableNewBetterJump)
                {
                    myRigidbody.velocity = new Vector2(myRigidbody.velocity.x*.7f, myRigidbody.velocity.y * 0.15f);
                }
                Walljumped = false;
                jumped = false;
                skoczylem = false;
            }

        }
    }

    public void onDodgeInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {       
            if (Time.time >= (lastDash + dashCoolDown) && FindObjectOfType<KontrolerGry>().currentStamina >= dashCost)
            {
                AttemptToDash();
            }
        }

    }

    public void onCrouchInput(InputAction.CallbackContext context)
    {



        if (context.performed)
        {
            isCrouching = true;
            headCollider.size = new Vector2(0.55f, 0.37f);
            headCollider.offset = new Vector2(0.05f, -0.11f);
        }

        if (context.canceled)
        {
            isCrouching = false;           
        }

    }

    public void onInteractInput(InputAction.CallbackContext context)
    {
        checkInteraction();
    }
  
    private void normalJump()
    {
       
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpSpeed);
        jumped = true;
    }
   
    public void StateMachine()
    {
        if (!isDashing)
        {
            if (!isAttacking)
            {
                if (!isHurt)
                {
                    if (col.onGround)
                    {
                        if (!isCrouching)
                        {
                            if (Input.GetAxisRaw("Horizontal") != 0)
                            {
                                ChangeAnimationState(Running);
                            }
                            else
                            {
                                ChangeAnimationState(Idling);
                            }

                        }
                        if (isCrouching)
                        {
                            ChangeAnimationState(Crouch);
                        }
                    }
                    else if (!col.onGround)
                    {

                        if (col.onWall)
                        {
                            ChangeAnimationState(WallSliding);

                        }
                        else if (skoczylem)
                        {
                            ChangeAnimationState(Jumping);
                        }
                        else
                        {
                            ChangeAnimationState(Jumping);
                        }
                    }
                }
                else
                {
                    // ChangeAnimationState(Hurting);
                    //Tutaj zaczynaj¹ siê animacje kiedy isHurt=Alive
                    if (col.onGround)
                    {
                        if (!isCrouching)
                        {
                            if (Input.GetAxisRaw("Horizontal") != 0)
                            {
                                ChangeAnimationState(HurtRunning);
                            }
                            else
                            {
                                ChangeAnimationState(HurtIdling);
                            }

                        }
                        if (isCrouching)
                        {
                            ChangeAnimationState(HurtCrouch);
                        }
                    }
                    else if (!col.onGround)
                    {

                        if (col.onWall)
                        {
                            ChangeAnimationState(HurtWallSliding);

                        }
                        else if (skoczylem)
                        {
                            ChangeAnimationState(HurtJumping);
                        }
                        else
                        {
                            ChangeAnimationState(HurtJumping);
                        }
                    }
                }
            }
            else
            {
                if (!isHurt)
                {
                    if (!col.onWall)
                    {
                        if (isFirstAttack)
                        {
                            ChangeAnimationState(PlayerAttack1);
                        }
                        else
                        {
                            ChangeAnimationState(PlayerAttack2);
                        }
                    }
                }
                else
                {
                    if (!col.onWall)
                    {
                        if (isFirstAttack)
                        {
                            ChangeAnimationState(HurtPlayerAttack1);
                        }
                        else
                        {
                            ChangeAnimationState(HurtPlayerAttack2);
                        }
                    }
                }

            }




        }
        else 
        {
            ChangeAnimationState(Dash);
        } 
    } 

    private void FlipSprite()
    {
        //float direction = Mathf.Round(myRigidbody.velocity.x);
        float direction = (Input.GetAxisRaw("Horizontal"));
        if (col.onGround)
        {
            if (direction < 0 && isFacingRight || direction > 0 && !isFacingRight)
            {
                isFacingRight = !isFacingRight;
                transform.Rotate(0, 180, 0);
            }
        }
        else if (!col.onGround && !col.onWall)
        {
            float directionNotGround = Mathf.Round(myRigidbody.velocity.x);
            if (directionNotGround < 0 && isFacingRight || directionNotGround > 0 && !isFacingRight)
            {
                isFacingRight = !isFacingRight;
                transform.Rotate(0, 180, 0);
            }
        }
        else if(col.onWall)
        {
            if(col.onLeftWall && isFacingRight)
            {
                isFacingRight = !isFacingRight;
                transform.Rotate(0, 180, 0);
            }
            else if(col.onRightWall && !isFacingRight)
             {
                isFacingRight = !isFacingRight;
                transform.Rotate(0, 180, 0);
             }
        }
    }
    bool IsFacingLeft()
    {
        return transform.localScale.x < 0;
    }

    private void Damage(float damage)
    {
        if (!isHurt)
        {
            if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy","Hazards")) || headCollider.IsTouchingLayers(LayerMask.GetMask("Enemy","Hazards")))
            {   
                if (IsFacingLeft())
                {
                    GetComponent<Rigidbody2D>().velocity = damageKickRight;
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = damageKickLeft;
                }
                isHurt = true;
                hurtCooldown = 2f;
                hazardCooldown = 1f;
                lastHurtTime = Time.time;
                FindObjectOfType<KontrolerGry>().TakingDamage(damage);
            }
        }
    }

    public void EnemyDamage(float damage)
    {
        if (!isHurt)
        {
            if (!isFacingRight)
            {
                GetComponent<Rigidbody2D>().velocity = damageKickRight;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = damageKickLeft;
            }
            isHurt = true;
            hurtCooldown = 2f;
            lastHurtTime = Time.time;
            FindObjectOfType<KontrolerGry>().TakingDamage(damage);
        }
    }

    public void HazardDamage(float damage, float hazardCoolDown, float moveCooldown)
    {
        if (!isHurt)
        {
            isHurt = true;
            hazardHurt = true;
            hurtCooldown = hazardCoolDown;
            hazardCooldown = moveCooldown;
            lastHurtTime = Time.time;
            FindObjectOfType<KontrolerGry>().TakingDamage(damage);
        }
    }

    public void CheckHurt()
    {
        if (isHurt && Time.time >= lastHurtTime + hurtCooldown)
        {
            isHurt = false;
        }
    }

    public void CheckHazard()
    {
        if (hazardHurt && Time.time >= lastHurtTime + hazardCooldown)
        {           
            hazardHurt = false;
        }
    }

    public void CharacterDeath()
    {
        isAlive = false;     
        myAnimator.SetTrigger("Dying");
        StartCoroutine(LoadSceneTimer());

    }

   
    IEnumerator LoadSceneTimer()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(currentScene);
    }    

    public void OpenInteractableIcon()
    {
        interactableIcon.SetActive(true);
    }

    public void CloseInteractableIcon()
    {
        interactableIcon.SetActive(false);
    }

    private void checkInteraction()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, interactBoxSize, 0, Vector2.zero);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D rc in hits)
            {
                if (rc.transform.GetComponent<Interactables>())
                {
                    rc.transform.GetComponent<Interactables>().Interact();
                    return;
                }
            }
        } 
    }


    private void deathlayertouch()
    {
        if (!isHurt)
        {
            if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Death")) || headCollider.IsTouchingLayers(LayerMask.GetMask("Death")))
            {

                FindObjectOfType<KontrolerGry>().TakingDamage(10f);
                isHurt = true;
                lastHurtTime = Time.time;
            }
        }
    }
}

