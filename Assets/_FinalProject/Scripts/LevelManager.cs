using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    // tracks the number of objectives to be completed to unlock the door
    // requiredPadsToRead should be set to the number of pads
    // in the scene in the inspection
    public int requiredPadsToRead = 0;
    private int padsRead;
    private HashSet<Transform> readPads;

    // if requiredPadsToRead is 0, the door will be unlocked immediately 
    void Start()
    {
        padsRead = 0;
        readPads = new HashSet<Transform>();

        if (requiredPadsToRead == 0)
        {
            EnableDoor();
        }
    }

    // the method to call when a pad is read and open the door if all pads are read
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

    // finds the door in the scene and unlocks it
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

