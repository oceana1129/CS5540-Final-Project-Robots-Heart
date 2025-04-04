using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
// this script is attached to the door object
// and handles the door's opening and closing behavior
public class DoorBehavior : MonoBehaviour
{
    public bool open;
    public bool isUnlocked = false;
    public GameObject spawnBackground;
    public float smooth = 1.0f;
    float DoorOpenAngle = -90.0f;
    float DoorCloseAngle = 0.0f;
    
    public AudioClip openDoor, closeDoor;
    AudioSource asource;

    // the door is closed by default and background is not spawned
    void Start()
    {
        asource = GetComponent<AudioSource>();
        if (!asource)
            Debug.LogError("audio source missing from door");
        
        spawnBackground.SetActive(false);
    }

    // the door will open or close depending on the current state
    void Update()
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

    // the method is referenced by PlayerInteraction
    // to control the audio and background
    public void OpenDoor()
    {
        if (isUnlocked)
        {
            open = !open;
            asource.clip = open ? openDoor : closeDoor;
            asource.Play();
            spawnBackground.SetActive(true);
        }
    }

    // the method is referenced by LevelManager to unlock the door
    public void UnlockDoor()
    {
        isUnlocked = true;
    }
}

