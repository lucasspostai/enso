using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour, IDamageable
{
    [HideInInspector] public float InvincibilityTime;
    [HideInInspector] public int Health;
    [HideInInspector] public int Balance;

    private void Update()
    {
        InvincibilityTime -= Time.deltaTime;
    }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Health = 0;
            Death();
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    public void UpdateBalance()
    {
        
    }
}
