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
    public event Action<float> onHealed;
    public event Action onDead;

    bool isDead = false;


    void Start()
    {
        currentHealth = maxHealth;
    }


    //取得目前血量
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    //取得總血量
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    //取得血量百分比
    public float GetHealthRatio()
    {
        return currentHealth / maxHealth;
    }
    //取得是否死亡
    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        if(gameObject.tag == "Player")
        {
            print("Player目前血量為：" + currentHealth);
        }

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        //如果血量>0會發放受到傷害的通知
        if (currentHealth > 0)
        {
            //?代表onDamage != null，代表有訂閱者（沒有訂閱者系統會報錯）
            onDamage?.Invoke();
        }

        if(currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    //如果血量<0會發放角色死亡的通知
    private void HandleDeath()
    {
        if (isDead) return;

        if (currentHealth <= 0)
        {
            isDead = true;
            onDead?.Invoke();
        }
    }

    //恢復血量
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }
}
