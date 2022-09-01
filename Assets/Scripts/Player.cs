using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject aim;
    [SerializeField] float speed;

    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    int attack = 2;
    int health = 5;

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

        MoveAnimation(direction);

        aim.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().SetDamage(attack);
        }
        else
        {
            MoveAnimation(rb.velocity);
        }
    }

    void MoveAnimation(Vector2 direction)
    {
        spriteRenderer.flipX = direction.x < 0;
        anim.SetBool("dash", true);
    }

    void SetDamage(int damage)
    {
        health -= damage;
    }
}
