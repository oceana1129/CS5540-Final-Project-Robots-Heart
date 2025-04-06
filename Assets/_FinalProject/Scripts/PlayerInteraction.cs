using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// This script handles player interactions with pads and doors in the game.
public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 2f;

    [Header("UI Elements")]
    public GameObject instructionTextUI;
    public GameObject infoTextUI;
    public GameObject lockedDoorMessageUI;

    [Header("Level Management")]
    public LevelManager levelManager;

    // Internal state
    private Transform currentInteractable;

    void Update()
    {
        CheckForInteraction();
        ClearInteractionIfOutOfRange();
    }

    // Checks for interaction with pads and doors within the interaction range
    void CheckForInteraction()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, interactionRange))
        {
            if (hit.transform.CompareTag("Pad"))
            {
                HandlePadInteraction(hit.transform);
            }
            else if (hit.transform.CompareTag("Door"))
            {
                HandleDoorInteraction(hit.transform);
            }
            else if (hit.transform.CompareTag("Milo"))
            {
                HandleMiloInteraction(hit.transform);
            }
            else
            {
                ResetInteractionUI();
            }
        }
        else
        {
            ResetInteractionUI();
        }
    }

    // Handles interaction with pads and toggles the info text UI (usually a pad)
    void HandlePadInteraction(Transform pad)
    {
        if (instructionTextUI != null) instructionTextUI.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (infoTextUI != null)
            {
                infoTextUI.SetActive(!infoTextUI.activeSelf);
                if (instructionTextUI != null) instructionTextUI.SetActive(!infoTextUI.activeSelf);
                currentInteractable = infoTextUI.activeSelf ? pad : null;

                if (infoTextUI.activeSelf)
                {
                    if (levelManager != null)
                    {
                        levelManager.PadRead(pad);
                    }
                }
            }
        }
    }

    // Handles interaction with doors and checks if they are unlocked
    void HandleDoorInteraction(Transform doorTransform)
    {
        if (instructionTextUI != null) instructionTextUI.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            DoorBehavior door = doorTransform.GetComponent<DoorBehavior>();
            if (door != null)
            {
                if (door.isUnlocked)
                {
                    door.OpenDoor();
                    if (instructionTextUI != null) instructionTextUI.SetActive(false);
                    currentInteractable = null;
                }
                else
                {
                    if (lockedDoorMessageUI != null)
                    {
                        lockedDoorMessageUI.SetActive(true);
                        StartCoroutine(HideLockedDoorMessageAfterDelay(2f));
                    }
                }
            }
        }
    }

    // Handles Milo interaction and opens the door
    void HandleMiloInteraction(Transform miloTransform)
    {
        if (instructionTextUI != null) instructionTextUI.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Animator miloAnimator = miloTransform.GetComponent<Animator>();
            if (miloAnimator != null)
            {
                miloAnimator.SetBool("Open_Anim", true);
                if (instructionTextUI != null) instructionTextUI.SetActive(false);

                if (infoTextUI != null)
                {
                    infoTextUI.SetActive(!infoTextUI.activeSelf);
                    currentInteractable = infoTextUI.activeSelf ? miloTransform : null;
                }
            }
        }
    }

    // Hides the locked door message after a delay
    IEnumerator HideLockedDoorMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (lockedDoorMessageUI != null)
        {
            lockedDoorMessageUI.SetActive(false);
        }
    }

    // Resets the interaction UI when not interacting with any object
    void ResetInteractionUI()
    {
        if (instructionTextUI != null) instructionTextUI.SetActive(false);
    }

    // Clears the current interactable object if it is out of range
    void ClearInteractionIfOutOfRange()
    {
        if (currentInteractable != null && Vector3.Distance(transform.position, currentInteractable.position) > interactionRange)
        {
            if (infoTextUI != null) infoTextUI.SetActive(false);
            if (lockedDoorMessageUI != null) lockedDoorMessageUI.SetActive(false);
            currentInteractable = null;
        }
    }

    // Draws a gizmo in the editor to visualize the interaction range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 rayOrigin = transform.position + Vector3.up;
        Vector3 rayDirection = transform.forward;

        Gizmos.DrawRay(rayOrigin, rayDirection * interactionRange);
    }
}

