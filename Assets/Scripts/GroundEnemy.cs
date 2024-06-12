using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class GroundEnemy : MonoBehaviour
{
    public float walkSpeed;
    public float walkStopRate = 0.05f;

    public ObjectDetection attackZone;
    public ObjectDetection cliffDetectionZone;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    Damageable damageable;

    public enum WalkableDirection {Right, Left};

    [SerializeField]
    private WalkableDirection _walkDirection = WalkableDirection.Right;
    private Vector2 walkDirectionVector = Vector2.right;
    

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                transform.localScale = new Vector2(gameObject.transform.localScale.x*-1, gameObject.transform.localScale.y);
                if (value == WalkableDirection.Right) { walkDirectionVector = Vector2.right; }
                else if (value == WalkableDirection.Left) { walkDirectionVector = Vector2.left; }
            }
            _walkDirection = value;
        }
    }


    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
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
    private bool _hasTarget = false;

    public bool HasTarget 
    {
        get { return _hasTarget; }
        private set 
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        } 
    }

    public bool CanMove 
    {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }

    private void Awake()
    {
        Transform child;
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        child = transform.Find("PlayerDetectionZone");
        attackZone = child.gameObject.GetComponent<ObjectDetection>();
        child = transform.Find("GroundDetectionZone");
        cliffDetectionZone = child.gameObject.GetComponent<ObjectDetection>();
    }

    private void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            if (touchingDirections.IsGrounded && touchingDirections.IsOnWall)
            {
                FlipDirection();
            }
            if (HasTarget) { rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y); }
            else { rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y); }
            IsMoving = rb.velocity != Vector2.zero;
        }
    }
    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right) { WalkDirection = WalkableDirection.Left; }
        else if (WalkDirection == WalkableDirection.Left) { WalkDirection = WalkableDirection.Right; }
        else { Debug.LogError("Current walk direction not set to right or left for entity"); }
        Debug.Log("I just flipped: " + touchingDirections.IsOnWall);
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected() 
    {
        if (touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
}
