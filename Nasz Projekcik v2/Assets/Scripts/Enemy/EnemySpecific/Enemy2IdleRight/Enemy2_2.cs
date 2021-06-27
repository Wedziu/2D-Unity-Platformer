using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2_2 : Entity
{
    public E2_2_IdleState idleState { get; private set; }
    public E2_2_PlayerDetectedState playerDetectedState { get; private set; }
    public E2_2_MeleeAttackState meleeAttackState { get; private set; }
    public E2_2_LookForPlayerState lookForPlayerState { get; private set; }
    public E2_2_StunState stunState { get; private set; }
    public E2_2_DeadState deadState { get; private set; }
    public E2_2_DodgeState dodgeState { get; private set; }
    public E2_2_RangedAttackState rangedAttackState { get; private set; }
    public Player player { get; private set; }


    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    public D_DodgeState dodgeStateData;
    [SerializeField]
    private D_RangedAttackState rangedAttackStateData;



    [SerializeField]
    private Transform meleeAttackPosition;
    [SerializeField]
    private Transform rangedAttackPosition;

    public override void Start()
    {
        base.Start();

        idleState = new E2_2_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new E2_2_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        meleeAttackState = new E2_2_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, player, this);
        lookForPlayerState = new E2_2_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        stunState = new E2_2_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new E2_2_DeadState(this, stateMachine, "dead", deadStateData, this);
        dodgeState = new E2_2_DodgeState(this, stateMachine, "dodge", dodgeStateData, this);
        rangedAttackState = new E2_2_RangedAttackState(this, stateMachine, "rangedAttack", rangedAttackPosition, rangedAttackStateData, this);

        Flip();
        stateMachine.Initialize(idleState);
    }

    public override void Damage(AttackDetails attackDeteals)
    {
        base.Damage(attackDeteals);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else if (isStuned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
        else if (CheckPlayerInMinAgroRange())
        {
            stateMachine.ChangeState(rangedAttackState);
        }
        else if (!CheckPlayerInMinAgroRange())
        {
            lookForPlayerState.SetTurnImmediately(true);
            stateMachine.ChangeState(lookForPlayerState);
        }
    }



    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
