using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : MonoBehaviour
{
    [Header("撿起來的補包")]
    [Tooltip("恢復的血量")] [SerializeField] float healAmount;
    [Tooltip("要摧毀的物件")] [SerializeField] GameObject pickupRoot;

    Pickup pickup;

    // Start is called before the first frame update
    void Start()
    {
        pickup = GetComponent<Pickup>();

        pickup.onPick += OnPick;
    }

    private void OnPick(GameObject player)
    {
        Health health = player.GetComponent<Health>();

        if (health)
        {
            print("恢復了" + healAmount + "血量");
            health.Heal(healAmount);

            Destroy(pickupRoot);
        }

    }
}
