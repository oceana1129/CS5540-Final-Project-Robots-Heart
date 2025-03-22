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
    public AudioClip damage1Sfx;
    public AudioClip damage2Sfx;
    public AudioClip damage3Sfx;
    public AudioClip heal1Sfx;
    public AudioClip heal2Sfx;

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
        if (audioSource)
        {
            float percentageOfHealthHealed = (heal / (float)maxHealth) * 100f; // Convert to float to prevent rounding issues

            if (percentageOfHealthHealed <= 50f)
                PlaySoundEffect(heal2Sfx);
            else
                PlaySoundEffect(heal1Sfx);
        }

        CurrentHealth += heal;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
        HandleAnimation();

        Debug.Log("current health " + CurrentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (audioSource)
        {
            float percentageOfHealthTaken = (damage / (float)maxHealth) * 100f; // Convert to float to prevent rounding issues

            if (percentageOfHealthTaken <= 50f)
                PlaySoundEffect(damage2Sfx);
            else
                PlaySoundEffect(damage1Sfx);
        }

        if (animator)
            animator.SetTrigger("isHit");

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

    void PlaySoundEffect(AudioClip sfx) 
    {
        if (sfx)
            audioSource.PlayOneShot(sfx);
    }

    void HandleAnimation() 
    {
        float energyLevels = Mathf.Clamp(((float)CurrentHealth / maxHealth) * 100f, 0f, 100f);

        animator.SetFloat("energyLevels", energyLevels);
    }
}
