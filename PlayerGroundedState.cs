using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
    }


    public override void Update()
    {
        base.Update();

        //* 滯空狀態
        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        //* 跳躍狀態
        if (Input.GetButtonDown("Jump") && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
    }


    public override void Exit()
    {
        base.Exit();
    }
}
