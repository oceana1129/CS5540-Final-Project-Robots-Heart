﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraDoorScript
{
    public class CameraOpenDoor : MonoBehaviour
    {
        public float DistanceOpen = 3f;
        public GameObject text;

        void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, DistanceOpen))
            {
                if (hit.transform.GetComponent<DoorScript.Door>())
                {
                    text.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        hit.transform.GetComponent<DoorScript.Door>().OpenDoor();
                    }
                }
                else
                {
                    text.SetActive(false);
                }
            }
            else
            {
                text.SetActive(false);
            }
        }
    }
}
