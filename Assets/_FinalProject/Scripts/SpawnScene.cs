using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class SpawnScene : MonoBehaviour
{
    public string nextSceneName;
    public string currentDoorID;
    private SceneManagement sceneManagement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        sceneManagement = FindAnyObjectByType<SceneManagement>();

        if (!sceneManagement)
            Debug.LogError("SceneManagement not found in scene.");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            Debug.Log("loading next scene: " + nextSceneName);
            EnterRoom();
            sceneManagement.LoadSceneByName(nextSceneName);
        }
        else 
        {
            Debug.Log("error loading next scene");
        }
    }

    public void EnterRoom()
    {
        if (currentDoorID == "" || currentDoorID == null)
        {
            Debug.LogWarning("there is no door id attached to this scene spawned");
        }
        SpawnManager.Instance.SetPreviousDoor(currentDoorID);
    }
}
