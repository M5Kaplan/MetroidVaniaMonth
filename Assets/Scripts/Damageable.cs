using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    Animator anim;
    

    [SerializeField] private float invincibilityTimer = 0.25f;
    private float timeSinceHit = 0;
    private bool isInvincible = false;

  



    [SerializeField] private int _maxHealth = 100;

    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField] private int _health = 100;
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
         
            _health = value;
            Debug.Log("_health set");
        }
    }
    private bool _isAlive = true;


    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            anim.SetBool(AnimationStrings.isAlive, value);
        }
    }
    public bool LockVelocity
    {
        get
        {
            return anim.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            anim.SetBool(AnimationStrings.lockVelocity, value);
        }

    }



    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        Death();
        InvincibleTimer();

    }

    public bool Hit(int damage,Vector2 knockback)
    {
        // eðer hayatta ve invincible timer bittiyse hit sonrasý damage alýr ve tekrar invincible timer baþlar.(invincible timer süresi boyunca damage almaz)
        if (IsAlive && !isInvincible)
        {
            _health -= damage;
            isInvincible = true;
            anim.SetTrigger(AnimationStrings.hit);
            LockVelocity = true;

            damageableHit?.Invoke(damage, knockback);
            return true;
        }else
        {
            return false;
        }
        

    }
    void Death()
    {
        if (_health <= 0)
        {
            Debug.Log("Health is below zero(0)");
            IsAlive = false;
        }
    }
    void InvincibleTimer()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTimer)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }
}

