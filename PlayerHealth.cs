using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    public Slider hpSlider;
    private bool isDead = false;

    void Start()
    {
        if (hpSlider != null) hpSlider.value = health;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        if (hpSlider != null) hpSlider.value = health;

        if (health <= 0f)
        {
            isDead = true;
            if (GameManager.instance != null)
            {
                GameManager.instance.LoseGame("You Died!");
            }
        }
    }
}