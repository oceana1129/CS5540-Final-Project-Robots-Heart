using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public GameObject pressETextUI;
    public GameObject infoTextUI;

    private bool isInteracting = false;

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, interactionRange))
        {
            if (hit.transform.CompareTag("Pad"))
            {
                pressETextUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    infoTextUI.SetActive(!infoTextUI.activeSelf);
                    pressETextUI.SetActive(!infoTextUI.activeSelf);
                    isInteracting = true;
                }
            }
            else if (hit.transform.CompareTag("Door"))
            {
                pressETextUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.GetComponent<DoorScript.DoorBehavior>().OpenDoor();
                    pressETextUI.SetActive(false);
                    isInteracting = true;
                }
            }
            else
            {
                pressETextUI.SetActive(false);
                isInteracting = false;
            }
        }
        else
        {
            pressETextUI.SetActive(false);
            isInteracting = false;
        }

        if (!isInteracting)
        {
            infoTextUI.SetActive(false);
        }
    }
}
