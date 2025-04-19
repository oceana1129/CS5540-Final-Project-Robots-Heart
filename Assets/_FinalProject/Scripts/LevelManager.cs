using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public bool isMiloObjective = false;
    public bool isReadingLogsObjective = false;
    public int requiredPadsToRead = 0;
    public string requiredFlagID; // The flag required to unlock the door

    private bool isMiloActivated = false;
    private bool isReadingLogsCompleted = false;
    private int padsRead;
    private HashSet<Transform> readPads;
    private bool doorUnlocked = false; // Track if the door has already been unlocked

    void Start()
    {
        if (isReadingLogsObjective)
        {
            padsRead = 0;
            readPads = new HashSet<Transform>();
        }
    }

    

    private void Update()
    {
        // Check if the required flag is set
        if (!doorUnlocked && !string.IsNullOrEmpty(requiredFlagID) && FlagManager.Instance.HasFlag(requiredFlagID))
        {
            EnableDoor();
            doorUnlocked = true; // Mark the door as unlocked
            return; // Exit to prevent further checks
        }

        // Existing logic for objectives
        if (isReadingLogsObjective && isMiloObjective)
        {
            if (isMiloActivated && isReadingLogsCompleted)
            {
                EnableDoor();
                doorUnlocked = true; // Mark the door as unlocked
            }
        }

        if (isMiloObjective && !isReadingLogsObjective)
        {
            if (isMiloActivated)
            {
                EnableDoor();
                doorUnlocked = true; // Mark the door as unlocked
            }
        }
        else if (isReadingLogsObjective && !isMiloObjective)
        {
            if (isReadingLogsCompleted)
            {
                EnableDoor();
                doorUnlocked = true; // Mark the door as unlocked
            }
        }
    }


    public void PadRead(Transform pad)
    {
        if (pad == null || readPads == null || isReadingLogsObjective == false)
        {
            return;
        }

        if (!readPads.Contains(pad))
        {
            readPads.Add(pad);
            padsRead++;

            if (padsRead >= requiredPadsToRead)
            {
                isReadingLogsCompleted = true;
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
            if (doorBehavior == null)
            {
                doorBehavior = door.GetComponentInParent<DoorBehavior>();
            }

            if (doorBehavior != null)
            {
                doorBehavior.UnlockDoor();
                return;
            }
        }
    }

    public void MiloActivated()
    {
        isMiloActivated = true;
    }
}
