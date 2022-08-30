using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//新增攻擊方式
public enum Actor
{
    Melee,
    Archer,
    Zombie,
}

public class Fighter : MonoBehaviour
{
    [Header("攻擊參數")]
    [Tooltip("角色攻擊類型")] [SerializeField] Actor actorType;
    [SerializeField] float attackDamage = 10;
    [SerializeField] float attackRange = 2;
    [SerializeField] float attackInterval = 2;

    [Tooltip("遠程攻擊彈藥")] [SerializeField] Projectile throwProjectile;
    [Tooltip("發出攻擊位置")] [SerializeField] Transform hand;

    Mover mover;
    Animator animator;
    Health health;
    Health targetHealth;

    float LastAttackDuration = Mathf.Infinity;

    void Start()
    {
        mover = GetComponent<Mover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();

        health.onDead += OnDead;
    }

    void Update()
    {
        //如果角色已死亡
        if (targetHealth == null || targetHealth.IsDead())
        {
            CancelTarget();
            return;
        }

        if (IsInAttackRange())
        {
            mover.CancelMove();
            AttackBehaviour();
        }
        else if (LastAttackDuration > attackInterval)
        {
            animator.SetBool("IsAttack", false);
            mover.MoveTo(targetHealth.transform.position, 1);
        }

        LastAttackDuration += Time.deltaTime;
    }

    //攻擊行為
    private void AttackBehaviour()
    {
        transform.LookAt(targetHealth.transform);

        if(LastAttackDuration > attackInterval)
        {
            LastAttackDuration = 0;
            TriggerAttack();
        }
    }


    //觸發攻擊動畫
    private void TriggerAttack()
    {
        animator.SetBool("IsAttack", true);
    }

    //動畫攻擊揮手才扣血
    private void Hit()
    {
        //沒有目標或是攻擊類型不是近戰
        if (targetHealth == null || actorType != Actor.Melee) return;

        if (IsInAttackRange())
        {
            targetHealth.TakeDamage(attackDamage);
        }

    }

    private void Shoot()
    {
        if (targetHealth == null || actorType != Actor.Archer) return;

        if (throwProjectile != null)
        {
            Projectile newProjectile = Instantiate(throwProjectile, hand.position, Quaternion.LookRotation(transform.forward));
            newProjectile.Shoot(gameObject);
        }
    }


    //檢查是否在攻擊範圍內
    private bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < attackRange;
    }

    public void Attack(Health target)
    {
        targetHealth = target;
    }

    public void CancelTarget()
    {
        animator.SetBool("IsAttack", false);
        targetHealth = null;
    }

    private void OnDead()
    {
        enabled = false;
    }

    //顯示攻擊範圍
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
