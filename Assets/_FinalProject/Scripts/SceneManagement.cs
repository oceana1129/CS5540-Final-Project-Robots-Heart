using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName;
    public string firstSceneName;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            Debug.LogError("no audio source attacjed to SceneManagement");
    }


    /// <summary>
    /// Call this when the player fails the scene.
    /// </summary>
    public void OnSceneFail()
    {
        Debug.Log("You lost");
    }

    /// <summary>
    /// Loads a scene by its name
    /// </summary>
    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// Loads the next scene.
    /// </summary>
    public void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.LogWarning("No next scene specified.");
    }

    /// <summary>
    /// Reloads the current scene.
    /// </summary>
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Restarts the game from the first scene.
    /// </summary>
    public void RestartGame()
    {
        if (!string.IsNullOrEmpty(firstSceneName))
            SceneManager.LoadScene(firstSceneName);
        else
            Debug.LogError("First scene name is not set in the inspector.");
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
