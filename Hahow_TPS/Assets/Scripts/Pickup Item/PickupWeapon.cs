using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    [Header("撿起來的武器控制")]
    [Tooltip("撿起來得到的武器")] [SerializeField] WeaponController weaponPrefab;

    Pickup pickup;

    void Start()
    {
        pickup = GetComponent<Pickup>();

        pickup.onPick += OnPick;
    }

    void Update()
    {
        
    }

    private void OnPick(GameObject player)
    {
        WeaponManager weaponManager = player.GetComponent<WeaponManager>();

        if (weaponManager)
        {
            if (weaponManager.AddWeapon(weaponPrefab))
            {
                if(weaponManager.GetActiveWeapon() == null)
                {
                    weaponManager.SwitchWeapon(1);
                }

                Destroy(gameObject);
            }
        }
    }
}
