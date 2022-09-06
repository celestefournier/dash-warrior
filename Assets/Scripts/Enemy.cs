using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] HealthBar healthBar;
    [SerializeField] Material hitMaterial;
    [SerializeField] Projectile projectilePrefab;

    int attack = 1;
    int maxHealth = 3;
    int health;
    bool firstDamage;

    SpriteRenderer sprite;
    Material materialDefault;
    IEnumerator damageCoroutine;

    void Awake()
    {
        health = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
        materialDefault = sprite.material;
    }

    void Start()
    {
        Attack();
    }

    public void Attack()
    {
        var willAttack = Random.Range(0f, 1f) > 0.5f;

        if (!willAttack)
            return;

        for (int j = 0; j < 8; j++)
        {
            var angle = Math.AngleToVector(360 / 8 * j);
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).Init(angle, attack);
        }
    }

    // IEnumerator AttackCoroutine()
    // {
    //     for (int j = 0; j < 8; j++)
    //     {
    //         var angle = Math.AngleToVector(360 / 8 * j);
    //         Instantiate(projectilePrefab, transform.position, Quaternion.identity).Init(angle, attack);
    //     }
    //
    //     int attackTimes = 1;
    //     
    //     for (int i = 0; i < attackTimes; i++)
    //     {
    //         for (int j = 0; j < 8; j++)
    //         {
    //             var angle = Math.AngleToVector(360 / 8 * j);
    //             Instantiate(projectilePrefab, transform.position, Quaternion.identity).Init(angle, attack);
    //         }
    //     
    //         if (i < attackTimes - 1)
    //         {
    //             yield return new WaitForSeconds(0.5f);
    //         }
    //     }
    // }

    public void SetDamage(int damage, Action onDie = null)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            Die();
            onDie?.Invoke();
        }

        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }

        damageCoroutine = DamageEffect();
        StartCoroutine(damageCoroutine);

        if (!firstDamage)
        {
            healthBar.gameObject.SetActive(true);
            firstDamage = true;
        }

        healthBar.SetValue(health, maxHealth);
    }

    IEnumerator DamageEffect()
    {
        sprite.material = hitMaterial;
        yield return new WaitForSeconds(0.2f);
        sprite.material = materialDefault;
        damageCoroutine = null;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}