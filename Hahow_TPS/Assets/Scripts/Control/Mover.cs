using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] float maxSpeed = 6;
    [SerializeField] float animationTransitionRatio = 0.1f;

    NavMeshAgent navMeshAgent;
    float lastFrameSpeed;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        UpdateAimatior();
    }

    private void UpdateAimatior()
    {
        //取得NavMesh的速度向量
        Vector3 velocity = navMeshAgent.velocity;
        //速度向量轉變為物件中心的向量
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, localVelocity.z, animationTransitionRatio);

        GetComponent<Animator>().SetFloat("WalkSpeed", lastFrameSpeed / (maxSpeed-1));
    }

    //移動到目標位置，以時間速率
    public void MoveTo(Vector3 destination, float speedRatio)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedRatio);
        navMeshAgent.destination = destination;
    }
    //取消移動
    public void CancelMove()
    {
        navMeshAgent.isStopped = true;
    }
}
