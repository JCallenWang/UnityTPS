using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ProjectileType
{
    Collider,
    Particle,
}

public class Projectile : MonoBehaviour
{
    [Header("子彈控制")]
    [Tooltip("子彈攻擊類型")] [SerializeField] ProjectileType type;
    [Tooltip("子彈飛行速度")] [SerializeField] float projectileSpeed;
    [Tooltip("子彈存活時間")] [SerializeField] float maxLifeTime = 3;
    [Tooltip("重力向下的力")] [SerializeField] float gravityDownForce = 0;
    [Tooltip("子彈攻擊傷害")] [SerializeField] float damage = 40;

    [Header("特效控制")]
    [Tooltip("子彈碰撞特效")] [SerializeField] GameObject hitParticle;
    [Tooltip("特效存活時間")] [SerializeField] float particleLifeTime = 2f;

    GameObject owner;
    bool canAttack;

    //飛行速度
    Vector3 currentVelocity;

    private void OnEnable()
    {
        Destroy(gameObject, maxLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += currentVelocity * Time.deltaTime;

        if(gravityDownForce > 0)
        {
            currentVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner || !canAttack) return;
        //if (other.CompareTag("Weapon") || other.CompareTag("Projectile")) return;
        if ((other.CompareTag("Enemy") || other.CompareTag("Player")) && type == ProjectileType.Collider)
        {
            Health targetHealth = other.gameObject.GetComponent<Health>();
            if (!targetHealth.IsDead())
            {
                targetHealth.TakeDamage(damage);
            }
        }

        HitEffect(transform.position);
        Destroy(gameObject);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other == owner || !canAttack) return;
        //if (other.CompareTag("Weapon") || other.CompareTag("Projectile")) return;
        if ((other.gameObject.tag == "Enemy" || other.CompareTag("Player")) && type == ProjectileType.Particle)
        {
            Health targetHealth = other.gameObject.GetComponent<Health>();
            if (!targetHealth.IsDead())
            {
                targetHealth.TakeDamage(damage);
            }
        }

        HitEffect(transform.position);
    }


    private void HitEffect(Vector3 Hitposition)
    {
        if (hitParticle)
        {
            GameObject newHitParticle = Instantiate(hitParticle, Hitposition, transform.rotation);
            if (particleLifeTime > 0)
            {
                Destroy(newHitParticle, particleLifeTime);
            }
        }
    }

    public void Shoot(GameObject gameObject)
    {
        owner = gameObject;
        currentVelocity = transform.forward * projectileSpeed;
        canAttack = true;
    }
}
