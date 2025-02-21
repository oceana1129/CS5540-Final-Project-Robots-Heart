using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class PlayerHealth : MonoBehaviour
{
    public static bool IsAlive{get; private set;}
    public int startingHealth = 100;
    private int currentHealth = 100;

    [Header("References")]
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = startingHealth;
        IsAlive = true;
    }

    public void HealDamage(int heal)
    {
        currentHealth += heal;
        currentHealth = Mathf.Clamp(currentHealth, 0, startingHealth);
        HandleAnimation();

        Debug.Log("current health " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, startingHealth);
        HandleAnimation();

        Debug.Log("current health " + currentHealth);
        if (currentHealth <= 0 && IsAlive)
        {
            PlayerDies();
        }
    }

    public void PlayerDies() 
    {
        Debug.Log("Player dies!");
        IsAlive = false;
    }

    void HandleAnimation() 
    {
        float energyLevels = Mathf.Clamp(((float)currentHealth / startingHealth) * 100f, 0f, 100f);

        animator.SetFloat("energyLevels", energyLevels);
    }
}
