using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    [SerializeField] float wayPointGizmosRadius = 1;

    //取得下一個巡邏點編號
    public int GetNextWayPointNumber(int wayPointNumber)
    {
        //檢查陣列0開始與實際數量1開始的差距
        if (wayPointNumber + 1 > transform.childCount - 1)
            return 0;
        return wayPointNumber + 1;
    }

    //取得巡邏點位置
    public Vector3 GetWayPointPosition(int wayPointNumber)
    {
        //GetChild出來為陣列（從0開始）
        return transform.GetChild(wayPointNumber).position;
    }

    //顯示所有巡邏點位置
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
