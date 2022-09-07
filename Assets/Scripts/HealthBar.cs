using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image bar;

    public void SetValue(float health, float maxHealth)
    {
        bar.fillAmount = health / maxHealth;
    }
}