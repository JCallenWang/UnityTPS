using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("速度參數")]
    [SerializeField] float speed = 5;
    [Range(1, 5)] [SerializeField] float sprintSpeedModifier = 3;
    [Range(0, 1)] [SerializeField] float crouchSpeedModifier = 0.5f;
    [SerializeField] float rotateSpeed = 5;
    [Tooltip("動畫切換參數比例")] [Range(0.01f, 0.1f)] [SerializeField] float AnimationTransitionRatio = 0.05f;
    [Header("跳躍參數")]
    [SerializeField] float jumpSpeed = 15;
    [SerializeField] float gravityForce = 50;
    [SerializeField] float distanceToGround = 0.01f;

    [Header("準星控制")]
    [SerializeField] GameObject crosshair;

    public event Action<bool> onAim;
    public event Action onSprint;


    InputController main_Input;
    CharacterController controller;
    Animator animator;
    Health health;

    Vector3 targetMovement;
    Vector3 jumpDirection;
    float lastFrameSpeed;
    bool isAim;
    bool isDead;

    private void Awake()
    {
        main_Input = GameManagerSingleton.Instance.InputController;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();

        health.onDead += OnDead;
    }

    private void Update()
    {
        if (isDead) return;

        AimBehaviour();
        MoveBehaviour();
        JumpBehaviour();
    }

    private void AimBehaviour()
    {
        bool lastTimeAim = isAim;

        if (main_Input.GetFireInputDown())
        {
            isAim = true;
        }
        if (main_Input.GetAimInputDown())
        {
            isAim = !isAim;
        }

        if(lastTimeAim != isAim)
        {
            if (crosshair != null) crosshair.SetActive(isAim);

            onAim?.Invoke(isAim);
        }


        animator.SetBool("IsAim", isAim);
    }

    private void MoveBehaviour()
    {
        targetMovement = Vector3.zero;
        targetMovement += main_Input.GetMoveInput().z * GetCameraForward();
        targetMovement += main_Input.GetMoveInput().x * GetCameraRight();
        targetMovement = Vector3.ClampMagnitude(targetMovement, 1);

        float nextFrameSpeed = 0;

        if (targetMovement == Vector3.zero)
        {
            nextFrameSpeed = 0f;
        }
        else if (main_Input.GetSprintInput() && !isAim)
        {
            nextFrameSpeed = 1;

            targetMovement *= sprintSpeedModifier;
            SmoothRotation(targetMovement);
            onSprint?.Invoke();
        }
        else if(!isAim)
        {
            nextFrameSpeed = 0.5f;

            SmoothRotation(targetMovement);
        }
        if (isAim)
        {
            SmoothRotation(GetCameraForward());
        }

        if (main_Input.GetCrouchInput())
        {
            targetMovement *= crouchSpeedModifier;
            animator.SetBool("IsCrouch", true);
        }
        else
        {
            animator.SetBool("IsCrouch", false);
        }

        if (lastFrameSpeed != nextFrameSpeed)
            lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, nextFrameSpeed, AnimationTransitionRatio);
        
        animator.SetFloat("WalkSpeed", lastFrameSpeed);
        animator.SetFloat("Vertical", main_Input.GetMoveInput().z);
        animator.SetFloat("Horizontal", main_Input.GetMoveInput().x);

        controller.Move(targetMovement * speed * Time.deltaTime);
    }
    private void JumpBehaviour()
    {
        if (main_Input.GetJumpInput() && IsGrounded() && !isAim)
        {
            animator.SetTrigger("IsJump");
            jumpDirection = Vector3.zero;
            jumpDirection += Vector3.up * jumpSpeed;
        }

        jumpDirection.y -= gravityForce * Time.deltaTime;
        jumpDirection.y = Math.Max(jumpDirection.y, -gravityForce);


        controller.Move(jumpDirection * Time.deltaTime);
    }


    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround);
    }
    private void SmoothRotation(Vector3 targetMove)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetMove, Vector3.up), rotateSpeed * Time.deltaTime);
    }


    private Vector3 GetCameraRight()
    {
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();
        return cameraRight;
    }
    private Vector3 GetCameraForward()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        return cameraForward;
    }

    private void OnDead()
    {
        animator.SetTrigger("IsDead");
        isDead = true;
    }
}
