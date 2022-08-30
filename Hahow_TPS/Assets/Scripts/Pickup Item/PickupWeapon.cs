using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    [Header("撿起來的武器控制")]
    [Tooltip("撿起來得到的武器")] [SerializeField] WeaponController weaponPrefab;

    Pickup pickup;

    // Start is called before the first frame update
    void Start()
    {
        pickup = GetComponent<Pickup>();

        pickup.onPick += OnPick;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPick(GameObject player)
    {
        WeaponManager weaponManager = player.GetComponent<WeaponManager>();

        //如果weaponManager存在
        if (weaponManager)
        {
            //如果成功新增武器
            if (weaponManager.AddWeapon(weaponPrefab))
            {
                //如果目前沒有啟用武器
                if(weaponManager.GetActiveWeapon() == null)
                {
                    //切換到下一個武器
                    weaponManager.SwitchWeapon(1);
                }

                //清除被撿起的武器
                Destroy(gameObject);
            }
        }
    }
}
