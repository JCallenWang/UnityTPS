using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] Image[] pocket;
    [SerializeField] Image[] weaponIcon;
    [SerializeField] Image[] energy;

    WeaponManager weaponManager;

    // Start is called before the first frame update
    void Start()
    {
        weaponManager = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponManager>();

        weaponManager.onAddWeapon += OnAddWeapon;
    }


    private void OnAddWeapon(WeaponController weaponController, int index)
    {
        weaponIcon[index].enabled = true;
        weaponIcon[index].sprite = weaponController.weaponicon;
    }

    private void Update()
    {
        for(int i = 0; i < 3; i++)
        {
            //continue : 繼續執行for迴圈，return : 不執行for迴圈
            if (weaponManager.GetWeaponAtSlotIndex(i) == null) continue;

            float value = weaponManager.GetWeaponAtSlotIndex(i).currentAmmoRatio;
            energy[i].fillAmount = Mathf.Lerp(energy[i].fillAmount, value, 0.2f);

            //凸顯當前武器
            if (weaponManager.GetWeaponAtSlotIndex(i) == weaponManager.GetActiveWeapon())
            {
                pocket[i].transform.localScale = Vector3.one;
                pocket[i].color = Color.white;
                weaponIcon[i].color = Color.white;
                energy[i].color = Color.white;
            }
            else
            {
                pocket[i].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                pocket[i].color = Color.gray;
                weaponIcon[i].color = Color.gray;
                energy[i].color = Color.gray;
            }
        }
    }
}
