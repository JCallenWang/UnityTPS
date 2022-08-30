using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("物件控制")]
    [Tooltip("物體上下浮動的頻率")] [SerializeField] float verticalFloatingFrequency = 1f;
    [Tooltip("物體上下浮動的距離")] [SerializeField] float floatingDistance = 1f;
    [Tooltip("物體每秒旋轉的角度")] [SerializeField] float rotatingSpeed = 360;

    //撿起Gameobject時通知所有訂閱者
    public event Action<GameObject> onPick;

    Rigidbody rigidbody;
    Collider collider;

    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        rigidbody.isKinematic = true;
        collider.isTrigger = true;

        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //上下移動的公式((Sin(時間*頻率)*速度)+修正) * 距離
        float floatingAnimationPhase = ((Mathf.Sin(Time.time * verticalFloatingFrequency) * 0.5f) + 0.5f) * floatingDistance;
        transform.position = startPosition + Vector3.up * floatingAnimationPhase;

        transform.Rotate(Vector3.up, rotatingSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            onPick?.Invoke(other.gameObject);
        }
    }
}
