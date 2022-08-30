using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    [SerializeField] float wayPointGizmosRadius = 1;

    public int GetNextWayPointNumber(int wayPointNumber)
    {
        if (wayPointNumber + 1 > transform.childCount - 1)
            return 0;
        return wayPointNumber + 1;
    }

    public Vector3 GetWayPointPosition(int wayPointNumber)
    {
        return transform.GetChild(wayPointNumber).position;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int j = GetNextWayPointNumber(i);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(GetWayPointPosition(i), wayPointGizmosRadius);
            Gizmos.DrawLine(GetWayPointPosition(i), GetWayPointPosition(j));
        }
    }
}
