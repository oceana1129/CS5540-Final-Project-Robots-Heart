using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerHealth : MonoBehaviour
{
    public static bool IsAlive{get; private set;}
    public int maxHealth = 100;
    private int CurrentHealth {get; set;} = 100;

    [Header("SFX Settings")]
    public AudioClip damage1;
    public AudioClip damage2;
    public AudioClip damage3;

    [Header("References")]
    private Animator animator;
    private AudioSource audioSource;

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        CurrentHealth = maxHealth;
        IsAlive = true;
    }

    public void HealDamage(int heal)
    {
        CurrentHealth += heal;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
        HandleAnimation();

        Debug.Log("current health " + CurrentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (audioSource)
            PlaySoundEffect(damage);

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
        HandleAnimation();

        Debug.Log("current health " + CurrentHealth);
        if (CurrentHealth <= 0 && IsAlive)
        {
            PlayerDies();
        }
    }

    public void PlayerDies() 
    {
        Debug.Log("Player dies!");
        IsAlive = false;
    }

    void PlaySoundEffect(int damage) 
    {
        float percentageOfHealthTaken = (damage / (float)maxHealth) * 100f; // Convert to float to prevent rounding issues

        if (percentageOfHealthTaken <= 50f)
            audioSource.PlayOneShot(damage2);
        else
            audioSource.PlayOneShot(damage1);
            
    }

    void HandleAnimation() 
    {
        float energyLevels = Mathf.Clamp(((float)CurrentHealth / maxHealth) * 100f, 0f, 100f);

        animator.SetFloat("energyLevels", energyLevels);
    }
}
