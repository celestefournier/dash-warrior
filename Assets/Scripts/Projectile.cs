using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitEffectPrefab;
    [SerializeField] float speed;

    int damage;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 position, int damage)
    {
        this.damage = damage;
        rb.velocity = position.normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag is "Player")
        {
            col.GetComponent<Player>().SetDamage(damage);
            Instantiate(hitEffectPrefab, col.ClosestPoint(transform.position), Quaternion.identity);
            Destroy(gameObject);
            return;
        }

        if (col.tag != "Enemy" && col.tag != "Projectile")
        {
            Instantiate(hitEffectPrefab, col.ClosestPoint(transform.position), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}