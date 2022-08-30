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

    public event Action<GameObject> onPick;

    Rigidbody rb;
    Collider col;

    Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        rb.isKinematic = true;
        col.isTrigger = true;

        startPosition = transform.position;
    }

    void Update()
    {
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
