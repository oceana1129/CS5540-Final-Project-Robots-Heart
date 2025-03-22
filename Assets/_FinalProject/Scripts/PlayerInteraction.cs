using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    IEnumerator HideLockedDoorMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (lockedDoorMessageUI != null)
        {
            lockedDoorMessageUI.SetActive(false);
        }
    }

    void ResetInteractionUI()
    {
        if (instructionTextUI != null) instructionTextUI.SetActive(false);
    }

    void ClearInteractionIfOutOfRange()
    {
        if (currentInteractable != null && Vector3.Distance(transform.position, currentInteractable.position) > interactionRange)
        {
            if (infoTextUI != null) infoTextUI.SetActive(false);
            if (lockedDoorMessageUI != null) lockedDoorMessageUI.SetActive(false);
            currentInteractable = null;
        }
    }
}
