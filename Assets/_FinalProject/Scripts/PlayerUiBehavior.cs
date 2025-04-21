using UnityEngine;

public class PlayerUiBehavior : MonoBehaviour
{
    [Header("UI GameObjects")]
    public GameObject interactUI;
    public GameObject doorUI;

    void Awake()
    {
        if (!interactUI || !doorUI)
        {
            Debug.LogWarning("UI components missing from player");
        }
        else 
        {
            interactUI.SetActive(false);
            doorUI.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interact"))
        {
            if (interactUI != null) interactUI.SetActive(true);
        }
        else if (other.CompareTag("Door"))
        {
            if (doorUI != null) doorUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interact"))
        {
            if (interactUI != null) interactUI.SetActive(false);
        }
        else if (other.CompareTag("Door"))
        {
            if (doorUI != null) doorUI.SetActive(false);
        }
    }
}
