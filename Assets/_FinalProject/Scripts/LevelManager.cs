using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public int requiredPadsToRead;
    private int padsRead;
    private HashSet<Transform> readPads;

    void Start()
    {
        padsRead = 0;
        readPads = new HashSet<Transform>();
    }

    public void PadRead(Transform pad)
    {
        if (pad == null)
        {
            return;
        }

        if (readPads == null)
        {
            return;
        }

        if (!readPads.Contains(pad))
        {
            readPads.Add(pad);
            padsRead++;

            if (padsRead >= requiredPadsToRead)
            {
                EnableDoor();
            }
        }
    }

    private void EnableDoor()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject door in doors)
        {
            DoorBehavior doorBehavior = door.GetComponent<DoorBehavior>();
            if (doorBehavior == null)
            {
                doorBehavior = door.GetComponentInChildren<DoorBehavior>();
            }

            if (doorBehavior != null)
            {
                doorBehavior.UnlockDoor();
                return;
            }
        }
    }
}
