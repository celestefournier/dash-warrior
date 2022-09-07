using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitEffectPrefab;
    [SerializeField] float speed;

    int damage;
    Action onDestroy;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 position, int damage, Action onDestroy)
    {
        this.damage = damage;
        this.onDestroy = onDestroy;
        rb.velocity = position.normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag is "Enemy" or "Projectile")
        {
            return;
        }

        if (col.attachedRigidbody.tag is "Player")
        {
            col.attachedRigidbody.GetComponent<Player>().SetDamage(damage);
        }

        Instantiate(hitEffectPrefab, col.ClosestPoint(transform.position), Quaternion.identity);
        onDestroy();
        Destroy(gameObject);
    }
}