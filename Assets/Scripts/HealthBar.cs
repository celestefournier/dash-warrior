using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Transform bar;

    float totalWidth;

    void Awake()
    {
        totalWidth = bar.localScale.x;
    }

    public void SetValue(float health, float maxHealth)
    {
        bar.localScale = new Vector3(
            health / maxHealth * totalWidth,
            bar.localScale.y,
            bar.localScale.z
        );
    }
}
