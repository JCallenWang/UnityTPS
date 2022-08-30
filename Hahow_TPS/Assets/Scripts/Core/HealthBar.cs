using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("取用元件")]
    [SerializeField] Health health;
    [SerializeField] GameObject rootCanvas;
    [SerializeField] Image foreGround;
    [Header("變更速率")]
    [Range(0, 1)] [SerializeField] float healthChangeRatio = 0.05f;


    void Update()
    {
        //當血量百分比 =0（死亡） or =1（滿血）
        if (Mathf.Approximately(health.GetHealthRatio(), 0) || Mathf.Approximately(health.GetHealthRatio(), 1))
        {
            rootCanvas.SetActive(false);
            return;
        }

        rootCanvas.SetActive(true);
        rootCanvas.transform.LookAt(Camera.main.transform.position);
        foreGround.fillAmount = Mathf.Lerp(foreGround.fillAmount, health.GetHealthRatio(), healthChangeRatio);
    }
}
