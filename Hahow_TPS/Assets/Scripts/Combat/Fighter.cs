using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void AttackBehaviour()
    {
        transform.LookAt(targetHealth.transform);

        if(LastAttackDuration > attackInterval)
        {
            LastAttackDuration = 0;
            TriggerAttack();
        }
    }


    private void TriggerAttack()
    {
        animator.SetBool("IsAttack", true);
    }

    private void Hit()
    {
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
