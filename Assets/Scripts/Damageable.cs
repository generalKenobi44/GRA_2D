using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent<int, int> healtChanged;
    
    Animator animator;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.75f;

    [SerializeField]
    private int _maxHealth;
    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    [SerializeField]
    private int _health = 100;
    public int Health
    {
        get { return _health; }
        set 
        { 
            _health = value;
            healtChanged?.Invoke(_health, MaxHealth);
            if (_health <= 0) { IsAlive = false; }
        }
    }

    [SerializeField]
    private bool _isAlive = true;
    public bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.IsAlive, value);
            Debug.Log("IsAlive set to: " + value);
        }
    }

    [SerializeField]
    private bool _isInvincible = false;

    public bool IsInvincible 
    {
        get { return _isInvincible; }
        private set { _isInvincible = value; }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (IsInvincible)
        {
            if(timeSinceHit > invincibilityTime)
            {
                IsInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !IsInvincible) 
        { 
            Health -= damage;
            IsInvincible = true;

            damageableHit?.Invoke(damage, knockback);
            CharacterActions.characterDamaged.Invoke(gameObject, damage);

            animator.SetTrigger(AnimationStrings.hit);
            return true;
        }
        return false;
    }

    public bool Heal(int healthRestored)
    {
        if (IsAlive && Health < MaxHealth)
        {
            Health = Mathf.Min(MaxHealth, Health + healthRestored);
            CharacterActions.characterHealed.Invoke(gameObject, healthRestored);
            return true;
        }
        return false;
    }
}
