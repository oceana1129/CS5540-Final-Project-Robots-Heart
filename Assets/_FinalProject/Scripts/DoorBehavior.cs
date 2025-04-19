using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorBehavior : MonoBehaviour
{
    public enum DoorType
    {
        Wooden,
        Electric,
        Rolling
    }

    [Header("Door General Setting")]
    public DoorType doorType;
    public bool open;
    public bool isUnlocked = false;
    public float smooth = 1.0f;
    public AudioClip openDoor, closeDoor;
    private AudioSource asource;

    [Header("Cooldown Settings")]
    public float cooldownTime = 2f; // Cooldown duration in seconds
    private float lastInteractionTime = -Mathf.Infinity; // Tracks the last interaction time

    [Header("Door Spawn Background Setting")]
    public bool requiresSpawnBackground = true;
    public GameObject spawnBackground;

    [Header("Wooden Door Setting")]
    private float DoorOpenAngle = -90.0f;
    private float DoorCloseAngle = 0.0f;

    [Header("Electric Door Setting")]
    public float slideDistance = 3.0f;
    public float slideSpeed = 2.0f;

    private Transform leftGate;
    private Transform rightGate;
    private Vector3 leftGateClosedPosition;
    private Vector3 rightGateClosedPosition;
    private Vector3 leftGateOpenPosition;
    private Vector3 rightGateOpenPosition;

    [Header("Rolling Door Setting")]
    public float rollDistance = 5.0f;
    public float rollSpeed = 2.0f;
    private Vector3 rollingDoorClosedPosition;
    private Vector3 rollingDoorOpenPosition;

    void Start()
    {
        asource = GetComponent<AudioSource>();
        if (!asource)
            Debug.LogError("Audio source missing from door");

        if (requiresSpawnBackground && spawnBackground != null)
        {
            spawnBackground.SetActive(false);
        }

        if (doorType == DoorType.Electric)
        {
            leftGate = transform.Find("LeftGate");
            rightGate = transform.Find("RightGate");

            if (leftGate == null || rightGate == null)
            {
                Debug.LogError("LeftGate or RightGate not found as children of the Gate object.");
                return;
            }

            leftGateClosedPosition = leftGate.localPosition;
            rightGateClosedPosition = rightGate.localPosition;

            leftGateOpenPosition = leftGateClosedPosition + Vector3.forward * slideDistance;
            rightGateOpenPosition = rightGateClosedPosition + Vector3.back * slideDistance;
        }
        else if (doorType == DoorType.Rolling)
        {
            rollingDoorClosedPosition = transform.localPosition;
            rollingDoorOpenPosition = rollingDoorClosedPosition + Vector3.down * rollDistance;
        }
    }

    void Update()
    {
        switch (doorType)
        {
            case DoorType.Wooden:
                HandleWoodenDoor();
                break;
            case DoorType.Electric:
                HandleElectricDoor();
                break;
            case DoorType.Rolling:
                HandleRollingDoor();
                break;
        }
    }

    void HandleWoodenDoor()
    {
        if (open)
        {
            var target = Quaternion.Euler(0, DoorOpenAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
        }
        else
        {
            var target1 = Quaternion.Euler(0, DoorCloseAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target1, Time.deltaTime * 5 * smooth);
        }
    }

    void HandleElectricDoor()
    {
        if (open)
        {
            leftGate.localPosition = Vector3.Lerp(leftGate.localPosition, leftGateOpenPosition, Time.deltaTime * slideSpeed);
            rightGate.localPosition = Vector3.Lerp(rightGate.localPosition, rightGateOpenPosition, Time.deltaTime * slideSpeed);
        }
        else
        {
            leftGate.localPosition = Vector3.Lerp(leftGate.localPosition, leftGateClosedPosition, Time.deltaTime * slideSpeed);
            rightGate.localPosition = Vector3.Lerp(rightGate.localPosition, rightGateClosedPosition, Time.deltaTime * slideSpeed);
        }
    }

    void HandleRollingDoor()
    {
        if (open)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, rollingDoorOpenPosition, Time.deltaTime * rollSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, rollingDoorClosedPosition, Time.deltaTime * rollSpeed);
        }
    }

    public void TryOpenDoor()
    {
        // Check if the door is unlocked
        if (!isUnlocked)
        {
            Debug.LogWarning("Door is locked and cannot be opened.");
            return;
        }

        // Check if the cooldown period has elapsed
        if (Time.time - lastInteractionTime < cooldownTime)
        {
            Debug.LogWarning("Door is on cooldown. Please wait.");
            return;
        }

        // Toggle the door's open state
        open = !open;

        // Play the appropriate sound
        asource.clip = open ? openDoor : closeDoor;
        asource.Play();

        // Activate the spawn background if required
        if (requiresSpawnBackground && spawnBackground != null && open)
        {
            spawnBackground.SetActive(true);
        }

        // Update the last interaction time
        lastInteractionTime = Time.time;
    }

    public void UnlockDoor()
    {
        isUnlocked = true;
        Debug.Log("Door unlocked.");
    }
}
