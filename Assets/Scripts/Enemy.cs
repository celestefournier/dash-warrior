using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] HealthBar healthBar;

    int maxHealth = 3;
    int health;
    bool firstDamage;

    void Awake()
    {
        health = maxHealth;
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

        if (!firstDamage)
        {
            healthBar.gameObject.SetActive(true);
            firstDamage = true;
        }

        healthBar.SetValue(health, maxHealth);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
