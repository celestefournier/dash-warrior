using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject aim;
    [SerializeField] float speed;
    [SerializeField] TMP_Text attackText;
    [SerializeField] TMP_Text healthText;

    Rigidbody2D rb;

    int attack = 2;
    int health = 5;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        attackText.text = attack.ToString();
        healthText.text = health.ToString();
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

        aim.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().SetDamage(attack, SetDamage);
        }
    }

    void SetDamage(int damage)
    {
        health -= damage;
        healthText.text = health.ToString();
    }
}
