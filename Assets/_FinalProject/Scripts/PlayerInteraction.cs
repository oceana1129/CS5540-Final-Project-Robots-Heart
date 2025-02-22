using DoorScript;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public GameObject instructionTextUI;
    public GameObject infoTextUI;

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
                HandlePadInteraction(hit.transform);
            else if (hit.transform.CompareTag("Door"))
                HandleDoorInteraction(hit.transform);
            else
                ResetInteractionUI();
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
                door.OpenDoor();
                if (instructionTextUI != null) instructionTextUI.SetActive(false);
                currentInteractable = null;
            }
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
            currentInteractable = null;
        }
    }
}
