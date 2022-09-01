using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] TMP_Text attackText;
    [SerializeField] TMP_Text healthText;

    int attack = 1;
    int health = 3;

    void Start()
    {
        attackText.text = attack.ToString();
        healthText.text = health.ToString();
    }

    public void SetDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            Die();
        }

        healthText.text = health.ToString();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
