using System;
using System.Collections;
using System.Collections.Generic;
using Races;
using UnityEngine;

public class EntityInfo : MonoBehaviour
{
    public string entityName;
    public float health;
    public float maxHealth;

    public float mana;
    public float maxMana;

    public Race race;
    public Sprite sprite;

    public bool HasMaxHealth => Math.Abs(health - maxHealth) < 0.1f;
    public bool HasMaxMana => Math.Abs(mana - maxMana) < 0.1f;

    public bool IsDead => Mathf.Floor(health) <= 0.1f;

    private bool _wasAliveLastFrame = true;
    private Animator _animator;
    private static readonly int Dying = Animator.StringToHash("dying");

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(entityName)}: {entityName}, {nameof(health)}: {health}";
    }

    private void Update()
    {
        if (_wasAliveLastFrame && IsDead)
        {
            Die();
        }
    }

    private void Die()
    {
        _wasAliveLastFrame = false;
        if (_animator)
        {
            _animator.SetBool(Dying, true);
        }
        
        // Remove bodies after X time
    }
}
