using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject aim;
    [SerializeField] float speed;
    [SerializeField] GameController gameController;
    [SerializeField] CameraShake cameraShake;
    [SerializeField] float minVel;

    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    bool isMoving;
    bool isAttacking;
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
        if (!gameController.playerTurn || isMoving)
            return;

        aim.SetActive(true);
    }

    void OnMouseDrag()
    {
        if (!gameController.playerTurn || isMoving)
            return;

        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var angle = Math.VectorToAngle(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        ) + 90;

        aim.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    void OnMouseUp()
    {
        if (!gameController.playerTurn || isMoving)
            return;

        isMoving = true;

        var angle = aim.transform.localEulerAngles.z + 90;
        var direction = Math.AngleToVector(angle);

        rb.AddForce(direction * speed);
        StartCoroutine(CheckEndTurn());
        DashAnimation(direction);
        aim.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!gameController.playerTurn)
            return;

        if (col.gameObject.tag == "Enemy")
        {
            isAttacking = true;
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
        if (direction.x > 0)
            spriteRenderer.flipX = false;
        else if (direction.x < 0)
            spriteRenderer.flipX = true;

        anim.SetBool("dash", true);
    }

    void OnAttackComplete()
    {
        isAttacking = false;
        rb.velocity = lastVelocity;
        DashAnimation(rb.velocity);
    }

    IEnumerator CheckEndTurn()
    {
        // Wait for velocity to process AddForce
        yield return new WaitForSeconds(0.1f);

        while (rb.velocity.magnitude > minVel || isAttacking)
        {
            yield return new WaitForSeconds(0.5f);
        }

        isMoving = false;
        rb.velocity = Vector2.zero;
        anim.SetBool("dash", false);
        // gameController.EndTurn(Turn.Player);
    }

    void SetDamage(int damage)
    {
        health -= damage;
    }
}
