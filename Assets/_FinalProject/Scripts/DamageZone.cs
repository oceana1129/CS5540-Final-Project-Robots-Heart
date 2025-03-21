using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DamageZone : MonoBehaviour
{
    public bool instantKill = false;        // toggle for instant death 
    public int damageAmount = 1;            // damage taken when hitting damage zone
    public float damageCooldown = 1.0f;    // cooldown time in seconds
    private bool canDamage = true;          // used for cooldowns


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
    }

    private IEnumerator DamageCooldownRoutine()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}