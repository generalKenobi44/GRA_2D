using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 7.5f;
    public float airWalkSpeed = 4f;
    public float airRunSpeed = 7f;
    public float jumpImpulse = 8f;

    private float dashTime = 0.3f;
    private float dashLockTime = 1.3f;
    private float timeSinceDash = 0f;

    public Vector2 moveInput;

    public TouchingDirections touchingDirections;
    public Rigidbody2D rb;
    public Animator animator;
    public Damageable damageable;

    private bool canAttack = true;

    [SerializeField]
    private float _dashSpeed = 15f;
    public float DashSpeed 
    {
        get 
        {
            if (IsFacingRight) { return _dashSpeed; }
            else if (!IsFacingRight) { return _dashSpeed * -1; }
            else { return 0; }
        }
    }

    public float CurrentMoveSpeed { 
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {

                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        return walkSpeed;
                    }
                    else
                    {
                        if (IsRunning)
                        {
                            return airRunSpeed;
                        }
                        return airWalkSpeed;
                    }
                }
                return 0;
            }
            return 0;
        }
        private set 
        {
            
        } 
    }

    public bool CanDash { get { return (timeSinceDash > dashLockTime); } }
    public bool IsAttacking { get { return animator.GetBool(AnimationStrings.isAttacking); } }


    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving { 
        get
        {
            return _isMoving;
        } 
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning { 
        get 
        { 
            return _isRunning;
        } 
        private set 
        { 
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        } 
    }

    [SerializeField]
    private bool _isDashing = false;
    public bool IsDashing 
    {
        get {  return _isDashing; }
        private set { _isDashing = value;}
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight { get { return _isFacingRight;  } 
        private set 
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        } 
    }
    public bool CanMove { 
        get 
        { 
            return animator.GetBool(AnimationStrings.canMove);
        } 
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            if (IsDashing)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                if (timeSinceDash > dashTime) { IsDashing = false; }
            }
            else
            {
                rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
            }
            animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        }
        if (timeSinceDash <= dashLockTime) { timeSinceDash += Time.deltaTime; }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if(damageable.IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            if (!IsDashing) { SetFacingDirection(moveInput); }
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        } else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && canAttack)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y); 
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && CanDash && CanMove) 
        {
            IsDashing = true;
            timeSinceDash = 0;
            rb.velocity = new Vector2(DashSpeed, 0);
            animator.SetTrigger(AnimationStrings.dash);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
