using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("血量參數")]
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;

    public event Action onDamage;
    public event Action onDead;

    bool isDead = false;


    void Start()
    {
        currentHealth = maxHealth;
    }


    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public float GetHealthRatio()
    {
        return currentHealth / maxHealth;
    }
    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;


        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth > 0)
        {
            onDamage?.Invoke();
        }

        if(currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        if (isDead) return;

        if (currentHealth <= 0)
        {
            isDead = true;
            onDead?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }
}
