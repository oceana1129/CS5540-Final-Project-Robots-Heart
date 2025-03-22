using UnityEngine;
using System.Collections;

public class HealZone : MonoBehaviour
{
    [Header("Heal Settings")]
    public bool instantHeal = false;        // toggle for full heal
    public int healAmount = 1;            // health gained when hitting heal zone
    public float healCooldown = 1.0f;    // cooldown time in seconds

    [Header("Destroy Object Settings")]
    public bool destroyOnContact = false;
    public float destroyTimer = 1f;

    // private variables
    private bool canHeal = true;          // used for cooldowns


    private void Start()
    {
        // gameobject must have at least one collider available
        Collider collider = GetComponent<Collider>();

        if (collider == null)
        {
            Debug.LogError($"[healZone] ERROR: No collider found on '{gameObject.name}'. Must attach collider with 'IsTrigger' enabled.");
            enabled = false; // disable script
            return;
        }

        // see if collider is disabled in inspector
        if (!collider.enabled)
        {
            Debug.LogError($"[HealZone] ERROR: Collider on '{gameObject.name}' is DISABLED. Please enable it.");
            enabled = false; // disable script
            return;
        }

        if (!collider.isTrigger)
        {
            Debug.LogError($"[HealZone] ERROR: Collider on '{gameObject.name}' does not have 'Is Trigger' enabled. Please enable it.");
            enabled = false;    // disable script
            return;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // PLAYER ENTERS HEAL ZONE
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered heal zone");
            HealPlayer(collision.gameObject);
        }
    }

    private void HealPlayer(GameObject player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();        // get player health

        // error if player health does not exist
        if (!playerHealth)
        {
            Debug.LogWarning("Player health component does not exist");
            return;
        }

        if (canHeal)                       // player health exists
        {
            int heal = instantHeal ? playerHealth.maxHealth : healAmount;   // calculate heal
            playerHealth.HealDamage(heal);            // player takes heal
            StartCoroutine(healCooldownRoutine());    // apply cooldown as needed
        }

        if (destroyOnContact)
            Destroy(gameObject, destroyTimer);
    }

    private IEnumerator healCooldownRoutine()
    {
        canHeal = false;
        yield return new WaitForSeconds(healCooldown);
        canHeal = true;
    }
}
