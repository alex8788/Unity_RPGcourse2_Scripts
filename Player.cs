using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move info")]
    public float moveSpeed = 12;
    public float jumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashCoolDown;
    private float dashCoolDownTimer;

    public float dashSpeed;
    public float dashDuration;
    public float dashDir {get; private set;}

    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;

    [Header("Collision info")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    #endregion


    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
    }


    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        stateMachine.Initialize(idleState);
    }


    private void Update()
    {
        stateMachine.currentState.Update();

        CheckDashInput();
    }


    public void SetVelocity (float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }


    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundMask);


    public void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0, 0);
    }


    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }


    private void CheckDashInput()
    {
        dashCoolDownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCoolDownTimer < 0)
        {
            dashCoolDownTimer = dashCoolDown;
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)  // 預設衝刺方向為面朝向
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
}
