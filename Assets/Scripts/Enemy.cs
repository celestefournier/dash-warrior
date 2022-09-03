using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] HealthBar healthBar;
    [SerializeField] Material hitMaterial;

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
