using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//透過[]與[index]改變來切換武器
public class WeaponManager : MonoBehaviour
{
    [Header("一開始就有的武器")]
    [SerializeField] List<WeaponController> startingWeapon = new List<WeaponController>();
    [Tooltip("裝備武器的地方")] [SerializeField] Transform equipWeaponPosition;
    [Tooltip("等待舉槍的時間")] [SerializeField] float waitForAimTime = 2;


    //啟用武器的編號
    int activeWeaponIndex;
    //是否在瞄準狀態
    bool isAim;

    public event Action<WeaponController, int> onAddWeapon;


    //限制武器數量，[3] = 0,1,2
    WeaponController[] weapon = new WeaponController[3];
    PlayerController playerController;
    InputController main_Input;


    void Start()
    {
        //-1超出weapon[]，代表不持有武器
        activeWeaponIndex = -1;

        playerController = GetComponent<PlayerController>();
        main_Input = GameManagerSingleton.Instance.InputController;
        playerController.onAim += OnAim;

        //新增初始武器到weapon[]裡
        foreach(WeaponController weapon in startingWeapon)
        {
            AddWeapon(weapon);
        }
        SwitchWeapon(1);

    }


    void Update()
    {
        //取得現在裝備的武器
        WeaponController activeWeapon = GetActiveWeapon();

        //成功取得武器編號且在瞄準狀態
        if(activeWeapon && isAim)
        {
            activeWeapon.HandleShootInput(
                main_Input.GetFireInputDown(),
                main_Input.GetFireInputHeld(),
                main_Input.GetFireInputUp()
            );
        }

        //處理切換武器
        int weaponSwitchInput = main_Input.GetSwitchWeapon();
        if (weaponSwitchInput != 0)
        {
            SwitchWeapon(weaponSwitchInput);
        }
    }

    //切換武器編號
    public void SwitchWeapon(int addIndex)
    {
        int newWeaponIndex;

        //比第一把武器Index大
        if (activeWeaponIndex + addIndex > weapon.Length - 1)
        {
            newWeaponIndex = 0;
        }
        //比最後一把武器Index小
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
    //切換武器
    private void SwitchToWeaponIndex(int newIndex)
    {
        if (newIndex >= 0 && newIndex < weapon.Length)
        {
            //weapon[newIndex]存在武器
            if (GetWeaponAtSlotIndex(newIndex) != null)
            {
                //隱藏目前啟動的武器
                if (GetActiveWeapon() != null)
                {
                    GetActiveWeapon().ShowWeapon(false);

                }
                //顯示新的武器
                activeWeaponIndex = newIndex;
                GetActiveWeapon().ShowWeapon(true);
            }
        }
    }


    //取得目前active的武器
    public WeaponController GetActiveWeapon()
    {
        return GetWeaponAtSlotIndex(activeWeaponIndex);
    }
    //取得在背包中的武器清單
    public WeaponController GetWeaponAtSlotIndex(int index)
    {
        //找到武器在背包中的編號位置，並回傳武器
        if (index >= 0 && index < weapon.Length - 1 && weapon[index] != null)
        {
            return weapon[index];
        }
        return null;
    }

    public bool AddWeapon(WeaponController weaponPrefabs)
    {
        //檢查新增武器使否已經存在
        if (HasWeapon(weaponPrefabs))
        {
            return false;
        }

        //找到沒有武器的空間
        for(int i = 0; i < weapon.Length; i++)
        {
            //如果有哪一個空間是空的
            if (weapon[i] == null)
            {
                //新增武器到預設位置
                WeaponController newWeaponInstance = Instantiate(weaponPrefabs,equipWeaponPosition);

                //來源物件設定為gameObject
                newWeaponInstance.sourcePrefab = weaponPrefabs.gameObject;
                newWeaponInstance.ShowWeapon(false);

                weapon[i] = newWeaponInstance;
                print("獲得新武器：" + weaponPrefabs.name);

                //當獲得新武器[i]時Invoke
                onAddWeapon?.Invoke(newWeaponInstance, i);

                return true;
            }
        }
        return false;
    }

    //檢查武器清單是否有一樣的武器
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


    //當接收到PlayerController.onAim.Invoke時，將回傳的bool值帶入到此函式
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
