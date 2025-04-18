using UnityEngine;

/// <summary>
/// Used to add flag behavior to an individual object
/// Must call the registerFlag function in other scripts to register
/// </summary>
public class FlagInteractionBehavior : MonoBehaviour
{
    [Header("Flag String Settings")]
    public string flagID;

    [Header("If Flag Need Another Flag")]
    public string requiredItemID;

    [Header("Disable If Item Is Flagged")]
    public bool disableIfFlagged = false;   // disable the object this is attached to if flagged
    public bool disableIfRequiredIdFound = false;
    public bool enableIfRequiredIdNeeded = false;   // enable this object if the required flag has been added

    [Header("Conditions (optional)")]
    public bool requiresTriggerEnter = false;   // requires trigger enter to be flagged
    public bool requiresInteract = false;       // requires an interaction to be flagged
    public bool requiresAnotherItem = false;    // requires an another item to be flagged
    private bool hasBeenFlagged = false; // if the item has been flagged
    private bool playerInRange = false;


    private void Start()
    {
        // Disable or change object based on flag state
        HandleInitialFlagChecks();
    }

    private void Update()
    {
        if (requiresInteract && playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryRegisterWithConditions();
        }
    }

    private void HandleInitialFlagChecks()
    {
        bool hasThisFlag = FlagManager.Instance.HasFlag(flagID);
        bool hasRequiredFlag = FlagManager.Instance.HasFlag(requiredItemID);

        if (disableIfFlagged && hasThisFlag)
        {
            gameObject.SetActive(false);
            return;
        }

        if (disableIfRequiredIdFound && hasRequiredFlag)
        {
            gameObject.SetActive(false);
            return;
        }

        if (enableIfRequiredIdNeeded && hasRequiredFlag)
        {
            gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Checks if conditions are met before registering the flag
    /// </summary>
    private void TryRegisterWithConditions()
    {
        if (requiresAnotherItem && !FlagManager.Instance.HasFlag(requiredItemID))
        {
            Debug.LogWarning($"Required flag {requiredItemID} not found for: {flagID}");
            return;
        }

        RegisterFlag();
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
        if (hasBeenFlagged)
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
        hasBeenFlagged = true;
        
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
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (requiresTriggerEnter && !requiresInteract)
        {
            TryRegisterWithConditions();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
    }
}
