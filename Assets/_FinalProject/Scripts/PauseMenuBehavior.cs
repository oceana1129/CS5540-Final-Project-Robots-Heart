using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuBehavior : MonoBehaviour
{
    [Header("Pause Menu Main Panel")]
    public GameObject pauseMenuPanel;
    public GameObject pauseContainer;
    public GameObject creditsContainer;
    public GameObject settingsContainer;
    public GameObject winContainer;

    [Header("Pause Menu Buttons")]
    public Button resumeButton;
    public Button quitButton;
    public Button settingsButton;
    public Button creditsButton;

    [Header("Credits Menu Buttons")]
    public Button creditsBackButton;

    [Header("Win Menu Buttons")]
    public Button winContinueButton;
    public Button winRestartButton;

    private SceneManagement sceneManagement;
    bool isGamePaused = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        sceneManagement = FindAnyObjectByType<SceneManagement>();

        if (!sceneManagement)
        {
            Debug.LogWarning("scene manager missing from scene");
            return;
        }

        if (FindAnyObjectByType<EventSystem>() == null)
        {
            Debug.LogWarning("No EventSystem found in the scene! UI buttons will not work.");
            return;
        }

        if (!pauseMenuPanel)
        {
            Debug.LogWarning("please add pause menu panel via the inspector.");
            return;
        }

        if (!pauseContainer || !creditsContainer)
        {
            Debug.LogWarning("please add containers for pause menu, credits menu, and win container via the inspector.");
            return;
        }

        CheckButtonAssignments();
        AssignButtonListeners();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleGamePaused();
        }
    }

    private void CheckButtonAssignments()
    {
        if (!resumeButton || !quitButton || !settingsButton || !quitButton)
        {
            Debug.LogWarning("Pause menu buttons not assigned via the inspector!");
            return;
        }

        if (!creditsBackButton) {
            Debug.LogWarning("Credits menu back button not assigned via the inspector!");
            return;
        }

        if (!creditsBackButton) {
            Debug.LogWarning("Win menu buttons not assigned via the inspector!");
            return;
        }
    }

    private void AssignButtonListeners()
    {
        if (resumeButton != null && resumeButton.onClick.GetPersistentEventCount() == 0)
            resumeButton.onClick.AddListener(ResumeGame);

        if (quitButton != null && quitButton.onClick.GetPersistentEventCount() == 0)
            quitButton.onClick.AddListener(QuitGame);

        if (settingsButton != null && settingsButton.onClick.GetPersistentEventCount() == 0)
            settingsButton.onClick.AddListener(ViewSettings);

        if (creditsButton != null && creditsButton.onClick.GetPersistentEventCount() == 0)
            creditsButton.onClick.AddListener(ViewCredits);

        if (creditsBackButton != null && creditsBackButton.onClick.GetPersistentEventCount() == 0)
            creditsBackButton.onClick.AddListener(ViewPauseMenu);

        if (winContinueButton != null && winContinueButton.onClick.GetPersistentEventCount() == 0)
            winContinueButton.onClick.AddListener(ResumeGame);

        if (winRestartButton != null && winRestartButton.onClick.GetPersistentEventCount() == 0)
            winRestartButton.onClick.AddListener(QuitGame);
    }

    public void ToggleGamePaused()
    {
        if (isGamePaused) 
        {
            ResumeGame();
        }
        else 
        {
            PauseGame();
        }
    }

    public void ResumeGame() 
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
    }

    public void PauseGame() 
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
        ViewPauseMenu();
    }

    
    public void QuitGame()
    {
        // SceneManager.LoadScene(0);
        // this is not used for webgl but works on other things
        // Application.Quit();
        Debug.Log("reloading the game");
        sceneManagement.LoadSceneByIndex(0);
    }

    public void ViewSettings()
    {
        // SceneManager.LoadScene(0);
        Debug.Log("open the settings menu");
    }

    public void ViewCredits()
    {
        Debug.Log("made by Oceana and Zuoyin");
        pauseContainer.SetActive(false);
        creditsContainer.SetActive(true);
        winContainer.SetActive(false);
    }

    public void ViewPauseMenu() {
        Debug.Log("looking at the pause menu");
        pauseContainer.SetActive(true);
        creditsContainer.SetActive(false);
        winContainer.SetActive(false);
    }

    public void ViewWinMenu() {
        Debug.Log("looking at the win menu");
        pauseContainer.SetActive(false);
        creditsContainer.SetActive(false);
        winContainer.SetActive(true);
    }

}
