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

    //上次發現玩家時間
    float lastSawPlayerTime = Mathf.Infinity;
    //起始位置
    Vector3 beginPosition;
    //目前巡邏點編號
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

        //訂閱Health.onDamage
        health.onDamage += OnDamage;
        health.onDead += OnDead;

    }

    private void Update()
    {
        //如果角色已經死亡就停止後續動作
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

    //攻擊行為
    private void AttackBehaviour()
    {
        animator.SetBool("IsConfuse", false);
        lastSawPlayerTime = 0;
        fighter.Attack(player.GetComponent<Health>());
    }
    //巡邏行為
    private void PatrolBehaviour()
    {
        if (patrolPath != null)
        {
            //如果到達當前巡邏點就開始計時
            if (IsAtWayPoint())
            {
                mover.CancelMove();
                arriveWayPointTime = 0;
                currentWayPoint = patrolPath.GetNextWayPointNumber(currentWayPoint);
            }
            //超過巡邏點時間就移動到下一個巡邏點
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
    //困惑行為
    private void ConfuseBehaviour()
    {
        mover.CancelMove();
        fighter.CancelTarget();
        animator.SetBool("IsConfuse", true);
    }

    //更新發現玩家時間＆抵達巡邏點時間
    private void UpdateTimer()
    {
        lastSawPlayerTime += Time.deltaTime;
        arriveWayPointTime += Time.deltaTime;
    }
    //檢查是否在追趕距離內
    private bool IsInChasingRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < stopChaseDistance;
    }
    //檢查是否在巡邏點容許範圍內
    private bool IsAtWayPoint()
    {
        return Vector3.Distance(transform.position, patrolPath.GetWayPointPosition(currentWayPoint)) < wayPointRange;
    }



    //訂閱Health.onDamage
    private void OnDamage()
    {
        isBeenAttack = true;
    }

    //訂閱Health.onDead
    private void OnDead()
    {
        mover.CancelMove();
        animator.SetTrigger("IsDead");
    }


    //顯示追逐範圍
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopChaseDistance);
    }
}
