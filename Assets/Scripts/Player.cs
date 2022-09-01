using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject aim;
    [SerializeField] float speed;
    [SerializeField] CameraShake cameraShake;

    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    int attack = 2;
    int health = 5;
    Vector2 lastVelocity;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        aim.SetActive(true);
    }

    void OnMouseDrag()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var angle = Math.VectorToAngle(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        ) + 90;

        aim.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    void OnMouseUp()
    {
        var angle = aim.transform.localEulerAngles.z + 90;
        var direction = Math.AngleToVector(angle);

        rb.AddForce(direction * speed);

        DashAnimation(direction);

        aim.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().SetDamage(attack);
            anim.SetTrigger("attack");
            lastVelocity = rb.velocity;
            rb.velocity = Vector2.zero;
            cameraShake.Shake();
        }
        else
        {
            DashAnimation(rb.velocity);
        }
    }

    void DashAnimation(Vector2 direction)
    {
        spriteRenderer.flipX = direction.x < 0;
        anim.SetBool("dash", true);
    }

    void OnAttackComplete()
    {
        rb.velocity = lastVelocity;
        DashAnimation(rb.velocity);
    }

    void SetDamage(int damage)
    {
        health -= damage;
    }
}
