using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponShootType
{
    Single,
    Automatic,
}

public class WeaponController : MonoBehaviour
{
    [Header("武器控制")]
    [Tooltip("武器Icon")] public Sprite weaponicon;
    [Tooltip("要顯示的武器")] [SerializeField] GameObject weaponRoot;
    [Tooltip("槍口位置")] [SerializeField] Transform weaponMuzzle;

    [Header("射擊控制")]
    [Tooltip("射擊類型")] [SerializeField] WeaponShootType shootType;
    [Tooltip("射擊間隔")] [SerializeField] float delayBetweenShoots = 0.5f;
    [Tooltip("連射武器射擊一發消耗的子彈數量")] [SerializeField] int bulletPerShoot;

    [Header("彈匣控制")]
    [Tooltip("子彈物件")] [SerializeField] Projectile projectilePrefab;
    [Tooltip("每秒彈匣補充的子彈速度")] [SerializeField] float ammoReloadRate = 1;
    [Tooltip("可以換彈的延遲時間")] [SerializeField] float ammoReloadDelay = 2;
    [Tooltip("最大子彈數量")] [SerializeField] int maxAmmo = 8;

    [Header("槍口特效")]
    [SerializeField] GameObject muzzleFlashPrefab;

    public GameObject sourcePrefab { get; set; }
    public float currentAmmoRatio { get; private set; }
    public bool isCooling { get; private set; }

    Health health;
    bool isDead;

    float currentAmmo;
    float timeSinceLastShoot;
    bool isAim;

    private void Awake()
    {
        currentAmmo = maxAmmo;
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        health.onDead += OnDead;
    }

    void Update()
    {
        UpdateAmmo();
    }

    private void UpdateAmmo()
    {
        if(timeSinceLastShoot + ammoReloadDelay < Time.time && currentAmmo < maxAmmo)
        {
            currentAmmo += ammoReloadRate * Time.deltaTime;

            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);

            isCooling = true;
        }
        else { isCooling = false; }

        if(maxAmmo == Mathf.Infinity)
        {
            currentAmmoRatio = 1;
        }
        else
        {
            currentAmmoRatio = currentAmmo / maxAmmo;
        }
    }

    public void ShowWeapon(bool value)
    {
        weaponRoot.SetActive(value);
    }

    public void HandleShootInput(bool inputDown, bool inputHeld, bool inputUp)
    {
        if (isDead) return;

        switch (shootType)
        {
            case WeaponShootType.Single:
                if (inputDown)
                {
                    TryShoot();
                }
                return;
            case WeaponShootType.Automatic:
                if (inputHeld)
                {
                    TryShoot();
                }
                return;
            default:
                return;
        }
    }

    private void TryShoot()
    {
        if (currentAmmo >= 1 && timeSinceLastShoot + delayBetweenShoots < Time.time)
        {
            HandleShoot();
            currentAmmo -= 1;
        }
    }

    private void HandleShoot()
    {
        for (int i = 0; i < bulletPerShoot; i++)
        {
            Projectile newProjectile = Instantiate(projectilePrefab, weaponMuzzle.position, Quaternion.LookRotation(weaponMuzzle.forward));
            newProjectile.Shoot(GameObject.FindGameObjectWithTag("Player"));
        }

        if(muzzleFlashPrefab != null)
        {
            GameObject newMuzzlePrefab = Instantiate(muzzleFlashPrefab, weaponMuzzle.position, weaponMuzzle.rotation, weaponMuzzle);
            Destroy(newMuzzlePrefab, 1.5f);
        }

        timeSinceLastShoot = Time.time;
    }

    private void OnDead()
    {
        isDead = true;
    }
}
