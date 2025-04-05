using UnityEngine;

public class SimpleAudioManager : MonoBehaviour
{
    public static SimpleAudioManager instance = null;
    public AudioSource bgmSource;
    public AudioClip bgmClip;

    void Awake()
    {
        // Ensure only one instance of SimpleAudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Play the BGM clip at the start
        if (bgmClip != null)
        {
            PlayBGM();
        }
        else
        {
            Debug.LogWarning("No BGM clip assigned.");
        }
    }

    public void PlayBGM()
    {
        if (bgmSource.clip != bgmClip)
        {
            bgmSource.clip = bgmClip;
        }
        bgmSource.Play();
        Debug.Log("Playing BGM clip: " + bgmClip.name);
    }

    public void StopBGM()
    {
        bgmSource.Stop();
        Debug.Log("BGM stopped.");
    }
}
