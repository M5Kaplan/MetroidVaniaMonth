using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections),typeof(Damageable))]

public class Knight_enemy : MonoBehaviour
{
    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float stopRate = .8f;

    Rigidbody2D rb;
    Animator anim;

    public enum WalkableDirection { Left, Right }
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    TouchingDirections touchingDirections;
    Damageable damageable;


    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                //turn the direction by * -1 scale
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.left;
                }
                else if
                    (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.right;
                }

            }
            _walkDirection = value;
        }
    }
    public DetectionZone attackZone;
    public bool _hasTarget = false;
    

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            anim.SetBool(AnimationStrings.hasTarget, value);

        }
    }
    public bool CanMove
    {
        get { return anim.GetBool(AnimationStrings.canMove); }
    }
 


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        anim = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (touchingDirections.IsOnWall && touchingDirections.IsGrounded)
        {
            Flip();
        }
        if (!damageable.LockVelocity)
        {
            if (CanMove)
            {
                rb.velocity = new Vector2(walkDirectionVector.x * walkSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, stopRate), rb.velocity.y);
            }
        }
      
        
    }
    private void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
    }

    void Flip()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Current walkable direction is not set to legal values of right or left");
        }
    }
    public void OnHit(int damage,Vector2 knockback)
    {    
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
