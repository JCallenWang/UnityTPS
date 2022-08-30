using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    [Header("玩家行為偵測")]
    [SerializeField] float stopChaseDistance = 10;
    [SerializeField] float startConfuseTime = 5;
    [Header("自動巡邏")]
    [SerializeField] PatrolPath patrolPath;
    [Tooltip("到達巡邏點的停留時間")] [SerializeField] float wayPointWaitTime = 3;
    [Tooltip("巡邏時的速度乘數")][Range(0, 1)] [SerializeField] float patrolSpeedRatio = 0.5f;
    [Tooltip("巡邏點容許範圍")] [SerializeField] float wayPointRange = 3;

    float lastSawPlayerTime = Mathf.Infinity;
    Vector3 beginPosition;
    int currentWayPoint = 0;
    float arriveWayPointTime = 0;
    bool isBeenAttack;

    GameObject player;
    Mover mover;
    Animator animator;
    Health health;
    Fighter fighter;
    
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        mover = GetComponent<Mover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        fighter = GetComponent<Fighter>();


        beginPosition = transform.position;

        health.onDamage += OnDamage;
        health.onDead += OnDead;

    }

    private void Update()
    {
        if (health.IsDead()) return;

        if (IsInChasingRange() || isBeenAttack)
        {
            AttackBehaviour();
        }
        else if (lastSawPlayerTime < startConfuseTime)
        {
            ConfuseBehaviour();
        }
        else
        {
            PatrolBehaviour();
        }

        UpdateTimer();
    }

    private void AttackBehaviour()
    {
        animator.SetBool("IsConfuse", false);
        lastSawPlayerTime = 0;
        fighter.Attack(player.GetComponent<Health>());
    }
    private void PatrolBehaviour()
    {
        if (patrolPath != null)
        {
            if (IsAtWayPoint())
            {
                mover.CancelMove();
                arriveWayPointTime = 0;
                currentWayPoint = patrolPath.GetNextWayPointNumber(currentWayPoint);
            }
            if(arriveWayPointTime > wayPointWaitTime)
            {
                animator.SetBool("IsConfuse", false);
                mover.MoveTo(patrolPath.GetWayPointPosition(currentWayPoint), patrolSpeedRatio);
            }
        }
        else
        {
            animator.SetBool("IsConfuse", false);
            mover.MoveTo(beginPosition, 0.3f);
        }
    }
    private void ConfuseBehaviour()
    {
        mover.CancelMove();
        fighter.CancelTarget();
        animator.SetBool("IsConfuse", true);
    }

    private void UpdateTimer()
    {
        lastSawPlayerTime += Time.deltaTime;
        arriveWayPointTime += Time.deltaTime;
    }
    private bool IsInChasingRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < stopChaseDistance;
    }
    private bool IsAtWayPoint()
    {
        return Vector3.Distance(transform.position, patrolPath.GetWayPointPosition(currentWayPoint)) < wayPointRange;
    }



    private void OnDamage()
    {
        isBeenAttack = true;
    }
    private void OnDead()
    {
        mover.CancelMove();
        animator.SetTrigger("IsDead");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopChaseDistance);
    }
}
