using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_ThirdPerson : MonoBehaviour
{
    InputController main_Input;

    [Header("跟隨目標物件")]
    [SerializeField] Transform target;
    [Header("滑鼠靈敏度")]
    [SerializeField] float sensitivity_XY = 2;
    [SerializeField] float sensitivity_wheel = 5;
    [Header("視野垂直角度")]
    [SerializeField] float minVerticalAngle = -10;
    [SerializeField] float maxVerticalAngle = 80;
    [Header("相機與目標距離")]
    [SerializeField] float cameraToTargetDistance = 10;
    [SerializeField] float minCameraDistance = 2;
    [SerializeField] float maxCameraDistance = 25;
    [Header("偏移量修正")]
    [SerializeField] Vector3 offset;

    [Header("玩家特效")]
    [SerializeField] GameObject player;
    [SerializeField] ParticleSystem beenHitParticle;
    [SerializeField] ParticleSystem sprintParticle;

    [Header("遊戲控制")]
    [SerializeField] GameObject pauseUI;


    //初始相機角度
    float mouse_X = 0;
    float mouse_Y = 10;

    private void Awake()
    {
        main_Input = GameManagerSingleton.Instance.InputController;
        player.GetComponent<Health>().onDamage += OnDamage;
        player.GetComponent<PlayerController>().onSprint += OnSprint;

    }

    private void LateUpdate()
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1;

            //垂直控制
            mouse_X += main_Input.GetMouseXAxis() * sensitivity_XY;
            mouse_Y += main_Input.GetMouseYAxis() * sensitivity_XY;
            mouse_Y = Mathf.Clamp(mouse_Y, minVerticalAngle, maxVerticalAngle);

            //可根據XY輸入自由旋轉
            transform.rotation = Quaternion.Euler(mouse_Y, mouse_X, 0);
            //根據物件中心+輸入旋轉的角度*往後的距離改變位置
            transform.position = target.position + Quaternion.Euler(mouse_Y, mouse_X, 0) * new Vector3(0, 0, -cameraToTargetDistance) + Vector3.up * offset.y;

            //距離控制
            cameraToTargetDistance += main_Input.GetMouseScrollWheel() * sensitivity_wheel;
            cameraToTargetDistance = Mathf.Clamp(cameraToTargetDistance, minCameraDistance, maxCameraDistance);
        }
        else
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnDamage()
    {
        if (beenHitParticle == null) return;

        beenHitParticle.Play();
    }

    private void OnSprint()
    {
        if (sprintParticle == null) return;

        sprintParticle.Play();
    }
}
