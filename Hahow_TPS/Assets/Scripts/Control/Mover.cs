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
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, localVelocity.z, animationTransitionRatio);

        GetComponent<Animator>().SetFloat("WalkSpeed", lastFrameSpeed / (maxSpeed-1));
    }

    public void MoveTo(Vector3 destination, float speedRatio)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedRatio);
        navMeshAgent.destination = destination;
    }
    public void CancelMove()
    {
        navMeshAgent.isStopped = true;
    }
}
