using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public string previousDoorID;

    void Awake()
    {
        if (previousDoorID == "") 
            Debug.LogWarning("door ID missing from this object");
    }

}
