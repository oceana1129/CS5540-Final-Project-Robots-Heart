using UnityEngine;

public class MainMenuBehavior : MonoBehaviour
{

    public GameObject menuContainer;
    public GameObject creditsContainer;
    public GameObject settingsContainer;
    private SceneManagement sceneManagement;

    private void Awake()
    {
        sceneManagement = FindAnyObjectByType<SceneManagement>();

        if (!sceneManagement)
        {
            Debug.LogWarning("scene manager missing from scene");
            return;
        }

        if (!menuContainer)
        {
            Debug.LogWarning("please add main menu panel via the inspector.");
            return;
        }
    }
    public void StartControls()
    {
        // SceneManager.LoadScene(0);
        Debug.Log("starting the main game");
        sceneManagement.LoadSceneByIndex(1);
    }

    public void StartGame()
    {
        // SceneManager.LoadScene(0);
        Debug.Log("starting the main game");
        sceneManagement.LoadSceneByIndex(2);
    }

    public void ViewSettings()
    {
        Debug.Log("open the settings menu");
        menuContainer.SetActive(false);
        settingsContainer.SetActive(true);
    }

    public void ViewCredits()
    {
        Debug.Log("made by Oceana and Zuoyin");
        menuContainer.SetActive(false);
        creditsContainer.SetActive(true);
    }

    public void ViewPauseMenu() {
        Debug.Log("looking at the pause menu");
        menuContainer.SetActive(true);
        creditsContainer.SetActive(false);
        settingsContainer.SetActive(false);
    }
}
