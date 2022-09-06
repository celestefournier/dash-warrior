using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject aim;
    [SerializeField] float speed;
    [SerializeField] GameController gameController;
    [SerializeField] GameObject hitEffectPrefab;
    [SerializeField] CameraShake cameraShake;
    [SerializeField] Material hitMaterial;

    bool isMoving;
    bool isAttacking;
    int attack = 1;
    int maxHealth = 5;
    int health;
    bool playerTurn;

    Vector2 lastVelocity;
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    Material materialDefault;
    IEnumerator damageCoroutine;

    void Awake()
    {
        health = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        materialDefault = sprite.material;
    }

    public void StartTurn()
    {
        playerTurn = true;
    }

    void OnMouseDown()
    {
        if (!playerTurn || isMoving)
            return;

        aim.SetActive(true);
    }

    void OnMouseDrag()
    {
        if (!playerTurn || isMoving)
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
        if (!playerTurn || isMoving)
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
        if (!playerTurn)
            return;

        if (col.gameObject.tag == "Enemy")
        {
            isAttacking = true;
            col.gameObject.GetComponent<Enemy>().SetDamage(attack);
            anim.SetTrigger("attack");
            lastVelocity = rb.velocity;
            rb.velocity = Vector2.zero;
            cameraShake.Shake();
            Instantiate(hitEffectPrefab, col.contacts[0].point, Quaternion.identity);
        }
        else
        {
            DashAnimation(rb.velocity);
        }
    }

    void DashAnimation(Vector2 direction)
    {
        if (direction.x > 0)
            sprite.flipX = false;
        else if (direction.x < 0)
            sprite.flipX = true;

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

        while (rb.velocity.magnitude > 1 || isAttacking)
        {
            yield return new WaitForSeconds(0.5f);
        }

        isMoving = false;
        playerTurn = false;
        rb.velocity = Vector2.zero;
        anim.SetBool("dash", false);
        gameController.EndTurn(Turn.Player);
    }

    public void SetDamage(int damage)
    {
        health -= damage;

        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }

        damageCoroutine = DamageEffect();
        StartCoroutine(damageCoroutine);
    }

    IEnumerator DamageEffect()
    {
        sprite.material = hitMaterial;
        yield return new WaitForSeconds(0.1f);
        sprite.material = materialDefault;
        damageCoroutine = null;
    }
}
