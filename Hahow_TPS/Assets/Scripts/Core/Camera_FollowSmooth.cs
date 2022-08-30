using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_FollowSmooth : MonoBehaviour
{
    //如何延遲跟隨物件

    [Header("跟隨目標物件")]
    [SerializeField] Transform target;
    [Header("與目標的最大距離")]
    [SerializeField] float distanceToTargetLimit;
    [Header("初始參數")]
    [Tooltip("一開始高度")] [SerializeField] float startHeight;
    [Tooltip("延遲的時間")] [SerializeField] float delayTime;
    [Header("Y軸偏移量修正")]
    [Tooltip("滾輪靈敏度")] [SerializeField] float sensitivityOffset_z;
    [Tooltip("Ｙ軸偏移量")] [SerializeField] float offset_y;
    [Tooltip("最小Ｙ軸高")] [SerializeField] float minAxisOffset_y;
    [Tooltip("最大Ｙ軸高")] [SerializeField] float maxAxisOffset_y;

    Vector3 smoothPosition = Vector3.zero;
    Vector3 currentVelocity = Vector3.zero;
    InputController main_Input;

    private void Awake()
    {
        transform.position = target.position + new Vector3(0, startHeight, 0);
        main_Input = GameManagerSingleton.Instance.InputController;
    }

    private void LateUpdate()
    {
        //滾輪更改物件高度
        if(main_Input.GetMouseScrollWheel() != 0)
        {
            offset_y += main_Input.GetMouseScrollWheel() * sensitivityOffset_z;
            offset_y = Mathf.Clamp(offset_y, minAxisOffset_y, maxAxisOffset_y);
        
            Vector3 nextMovement = target.position + target.up * offset_y;
            transform.position = Vector3.Lerp(transform.position, nextMovement, delayTime);
            //transform.position = target.position + target.up * offset_y;
        }

        //超過容許距離使物件開始移動
        if (CheckDistance())
        {
            smoothPosition = Vector3.SmoothDamp(transform.position, target.position + target.up * startHeight, ref currentVelocity, delayTime);
            transform.position = smoothPosition;
        }
    }

    //檢查與目標物件的距離是否超過容許值
    private bool CheckDistance()
    {
        return Vector3.Distance(transform.position, target.position) > distanceToTargetLimit;
    }
}
