using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject aim;
    [SerializeField] float speed;
    [SerializeField] float drag;

    Rigidbody2D rb;
    Vector2 velocity;
    IEnumerator moveCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

        // Move(direction * speed, drag);

        aim.SetActive(false);
    }

    // void OnCollisionEnter2D(Collision2D col)
    // {
    //     var direction = Vector2.Reflect(velocity.normalized, col.contacts[0].normal) * velocity.magnitude;

    //     Move(direction, drag);
    // }

    // void Move(Vector2 direction, float deceleration)
    // {
    //     if (moveCoroutine != null)
    //         StopCoroutine(moveCoroutine);

    //     moveCoroutine = MoveCoroutine(direction, deceleration);
    //     StartCoroutine(moveCoroutine);
    // }

    // IEnumerator MoveCoroutine(Vector2 direction, float deceleration)
    // {
    //     velocity = direction;

    //     while (velocity != Vector2.zero)
    //     {
    //         var radius = velocity.magnitude - deceleration;

    //         if (radius < 0.001f)
    //         {
    //             velocity = Vector2.zero;
    //             break;
    //         }

    //         velocity = velocity.normalized * radius;

    //         var distancia = (Vector2) transform.position + velocity * Time.fixedDeltaTime;
    //         rb.MovePosition(distancia);

    //         yield return 0;
    //     }
    // }
}
