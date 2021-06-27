using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_1_IdleState : IdleState
{
    private Enemy2_1 enemy;

    public E2_1_IdleState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Enemy2_1 enemy) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
