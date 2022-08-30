using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("一開始就有的武器")]
    [SerializeField] List<WeaponController> startingWeapon = new List<WeaponController>();
    [Tooltip("裝備武器的地方")] [SerializeField] Transform equipWeaponPosition;
    [Tooltip("等待舉槍的時間")] [SerializeField] float waitForAimTime = 2;


    int activeWeaponIndex;
    bool isAim;

    public event Action<WeaponController, int> onAddWeapon;


    WeaponController[] weapon = new WeaponController[3];
    PlayerController playerController;
    InputController main_Input;


    void Start()
    {
        activeWeaponIndex = -1;

        playerController = GetComponent<PlayerController>();
        main_Input = GameManagerSingleton.Instance.InputController;
        playerController.onAim += OnAim;

        foreach(WeaponController weapon in startingWeapon)
        {
            AddWeapon(weapon);
        }
        SwitchWeapon(1);

    }


    void Update()
    {
        WeaponController activeWeapon = GetActiveWeapon();

        if(activeWeapon && isAim)
        {
            activeWeapon.HandleShootInput(
                main_Input.GetFireInputDown(),
                main_Input.GetFireInputHeld(),
                main_Input.GetFireInputUp()
            );
        }

        int weaponSwitchInput = main_Input.GetSwitchWeapon();
        if (weaponSwitchInput != 0)
        {
            SwitchWeapon(weaponSwitchInput);
        }
    }

    public void SwitchWeapon(int addIndex)
    {
        int newWeaponIndex;

        if (activeWeaponIndex + addIndex > weapon.Length - 1)
        {
            newWeaponIndex = 0;
        }
        else if (activeWeaponIndex + addIndex < 0)
        {
            newWeaponIndex = weapon.Length - 1;
        }
        else
        {
            newWeaponIndex = activeWeaponIndex + addIndex;
        }

        SwitchToWeaponIndex(newWeaponIndex);
    }
    private void SwitchToWeaponIndex(int newIndex)
    {
        if (newIndex >= 0 && newIndex < weapon.Length)
        {
            if (GetWeaponAtSlotIndex(newIndex) != null)
            {
                if (GetActiveWeapon() != null)
                {
                    GetActiveWeapon().ShowWeapon(false);

                }
                activeWeaponIndex = newIndex;
                GetActiveWeapon().ShowWeapon(true);
            }
        }
    }


    public WeaponController GetActiveWeapon()
    {
        return GetWeaponAtSlotIndex(activeWeaponIndex);
    }
    public WeaponController GetWeaponAtSlotIndex(int index)
    {
        if (index >= 0 && index < weapon.Length - 1 && weapon[index] != null)
        {
            return weapon[index];
        }
        return null;
    }

    public bool AddWeapon(WeaponController weaponPrefabs)
    {
        if (HasWeapon(weaponPrefabs))
        {
            return false;
        }

        for(int i = 0; i < weapon.Length; i++)
        {
            if (weapon[i] == null)
            {
                WeaponController newWeaponInstance = Instantiate(weaponPrefabs,equipWeaponPosition);

                newWeaponInstance.sourcePrefab = weaponPrefabs.gameObject;
                newWeaponInstance.ShowWeapon(false);

                weapon[i] = newWeaponInstance;

                onAddWeapon?.Invoke(newWeaponInstance, i);

                return true;
            }
        }
        return false;
    }

    private bool HasWeapon(WeaponController weaponPrefabs)
    {
        foreach(WeaponController weaponCheck in weapon)
        {
            if(weaponCheck != null && weaponCheck.sourcePrefab == weaponPrefabs)
            {
                return true;
            }
        }
        return false;
    }


    private void OnAim(bool value)
    {
        if (value)
        {
            StopAllCoroutines();
            StartCoroutine(DelayAim());
        }
        else
        {
            StopAllCoroutines();
            isAim = value;
        }
    }

    IEnumerator DelayAim()
    {
        yield return new WaitForSecondsRealtime(waitForAimTime);
        isAim = true;
    }

}
