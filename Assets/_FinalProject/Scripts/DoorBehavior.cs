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

    // isUnlocked should be false if the door is free to open
    [Header("Door General Setting")]
    public DoorType doorType;
    public bool open;
    public bool isUnlocked = false;
    public float smooth = 1.0f;
    public AudioClip openDoor, closeDoor;
    AudioSource asource;

    // spawn background used for gate doors
    [Header("Door Spawn Background Setting")]
    public bool requiresSpawnBackground = true;
    public GameObject spawnBackground;
    
    [Header("Wooden Door Setting")]
    float DoorOpenAngle = -90.0f;
    float DoorCloseAngle = 0.0f;

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
            Debug.LogError("audio source missing from door");

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

    public void OpenDoor()
    {
        if (isUnlocked)
        {
            open = !open;
            asource.clip = open ? openDoor : closeDoor;
            asource.Play();
            if (requiresSpawnBackground && spawnBackground != null)
            {
                spawnBackground.SetActive(true);
            }
        }
    }

    public void UnlockDoor()
    {
        isUnlocked = true;
    }
}
