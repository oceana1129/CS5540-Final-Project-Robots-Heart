using UnityEngine;
using System.Collections;

public class DamageZone : MonoBehaviour
{
    [Header("Damage Settings")]
    public bool instantKill = false;        // toggle for instant death 
    public int damageAmount = 1;            // damage taken when hitting damage zone
    public float damageCooldown = 1.0f;    // cooldown time in seconds

    [Header("Destroy Object Settings")]
    public bool destroyOnContact = false;
    public float destroyTimer = 1f;

    // private variables
    private bool canDamage = true;          // used for cooldowns


    private void Start()
    {
        // gameobject must have at least one collider available
        Collider collider = GetComponent<Collider>();

        if (collider == null)
        {
            Debug.LogError($"[DamageZone] ERROR: No collider found on '{gameObject.name}'. Must attach collider with 'IsTrigger' enabled.");
            enabled = false; // disable script
            return;
        }

        // see if collider is disabled in inspector
        if (!collider.enabled)
        {
            Debug.LogError($"[DamageZone] ERROR: Collider on '{gameObject.name}' is DISABLED. Please enable it.");
            enabled = false; // disable script
            return;
        }

        if (!collider.isTrigger)
        {
            Debug.LogError($"[DamageZone] ERROR: Collider on '{gameObject.name}' does not have 'Is Trigger' enabled. Please enable it.");
            enabled = false;    // disable script
            return;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // PLAYER ENTERS DAMAGE ZONE
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered damage zone");
            DamagePlayer(collision.gameObject);
        }
    }

    private void DamagePlayer(GameObject player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();        // get player health

        // error if player health does not exist
        if (!playerHealth)
        {
            Debug.LogWarning("Player health component does not exist");
            return;
        }

        if (canDamage)                       // player health exists
        {
            int damage = instantKill ? playerHealth.maxHealth : damageAmount;   // calculate damage
            playerHealth.TakeDamage(damage);            // player takes damage
            StartCoroutine(DamageCooldownRoutine());    // apply cooldown as needed
        }

        if (destroyOnContact)
            Destroy(gameObject, destroyTimer);
    }

    private IEnumerator DamageCooldownRoutine()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}