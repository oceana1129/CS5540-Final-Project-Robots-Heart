using UnityEngine;

/// <summary>
/// Used to add flag behavior to an individual object
/// Must call the registerFlag function in other scripts to register
/// </summary>
public class FlagInteractionBehavior : MonoBehaviour
{
    [Header("Flag Settings")]
    public string flagID;

    [Header("Conditions (optional)")]
    public string requiredItemID;
    public bool disableIfFlagged = false;
    public bool requiresTriggerEnter = false;
    public bool requiresInteract = false;
    public bool requiresAnotherItem = false;
    private bool hasInteracted = false;


    private void Start()
    {
        // Disable or change object based on flag state
        DeactivateObjectIfFlagged();
    }

    /// <summary>
    /// Deactivate the current gameobject if it should not exist in the scene if flagged
    /// For example, could be for a key or puzzle
    /// </summary>
    void DeactivateObjectIfFlagged()
    {
        if (disableIfFlagged && FlagManager.Instance.HasFlag(flagID))
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Register this gameobjects flag
    /// Can trigger a flag via another script
    /// </summary>
    public void RegisterFlag()
    {
        if (hasInteracted)
        {
            Debug.LogWarning(flagID + " has already been registered");
            return;
        }

        if (string.IsNullOrEmpty(flagID))
        {
            Debug.LogWarning("No flagID assigned to " + gameObject.name);
            return;
        }

        FlagManager.Instance.AddFlag(flagID);
        hasInteracted = true;
        
        Debug.Log("added flag for " + flagID);
        if (disableIfFlagged)
            gameObject.SetActive(false);
    }

    /// <summary>
    /// Auto flag on trigger enter
    /// For example, walking through a door
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (requiresTriggerEnter && other.CompareTag("Player"))
        {
            RegisterFlag();
        }
    }
}
