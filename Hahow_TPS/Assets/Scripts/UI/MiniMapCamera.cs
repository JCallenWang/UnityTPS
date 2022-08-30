using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 positionOffset;

    private void Start()
    {
        transform.position = target.position + positionOffset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + positionOffset;
    }
}
