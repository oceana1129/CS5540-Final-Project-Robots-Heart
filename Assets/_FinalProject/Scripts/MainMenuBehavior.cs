using UnityEngine;

public class MainMenuBehavior : MonoBehaviour
{
    public GameObject mainMenuPanel;
    private SceneManagement sceneManagement;

    private void Awake()
    {
        sceneManagement = FindAnyObjectByType<SceneManagement>();

        if (!sceneManagement)
        {
            Debug.LogWarning("scene manager missing from scene");
            return;
        }

        if (!mainMenuPanel)
        {
            Debug.LogWarning("please add main menu panel via the inspector.");
            return;
        }
    }
    public void StartGame()
    {
        // SceneManager.LoadScene(0);
        Debug.Log("starting the main game");
        sceneManagement.LoadSceneByIndex(1);
    }

    public void ViewSettings()
    {
        // SceneManager.LoadScene(0);
        Debug.Log("open the settings menu");
        // this is not used for webgl but works on other things
        // Application.Quit();
    }

    public void ViewCredits()
    {
        Debug.Log("made by Oceana and Zuoyin");
    }
}
