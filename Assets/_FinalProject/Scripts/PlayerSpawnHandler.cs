using UnityEngine;

public class PlayerSpawnHandler : MonoBehaviour
{
    void Start()
    {
        string previousDoor = SpawnManager.Instance.GetPreviousDoor();
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        bool spawned = false;

        if (!string.IsNullOrEmpty(previousDoor))
        {
            foreach (var point in spawnPoints)
            {
                if (point.previousDoorID == previousDoor)
                {
                    transform.position = point.transform.position;
                    transform.rotation = point.transform.rotation;
                    Debug.Log("Player spawned at: " + point.previousDoorID);
                    spawned = true;
                    break;
                }
            }

            if (!spawned)
            {
                Debug.LogWarning("No spawn point found for previous door ID: " + previousDoor);
            }
        }

        // fallback to default spawn if a previous door id was not stated
        if (!spawned)
        {
            foreach (var point in spawnPoints)
            {
                if (point.previousDoorID.ToLower() == "default")
                {
                    transform.position = point.transform.position;
                    transform.rotation = point.transform.rotation;
                    Debug.Log("Player spawned at default location");
                    spawned = true;
                    break;
                }
            }

            if (!spawned)
            {
                Debug.LogError("No valid spawn point found or defauly point... player may spawn at origin.");
            }
        }
    }
}
