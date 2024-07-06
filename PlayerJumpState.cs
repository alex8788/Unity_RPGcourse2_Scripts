using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();

        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }


    public override void Update()
    {
        base.Update();  

        //* 滯空狀態
        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);
    }


    public override void Exit()
    {
        base.Exit();
    }
}
