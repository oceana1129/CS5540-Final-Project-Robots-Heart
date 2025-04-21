using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider volumeSlider;

    private void Start()
    {
        // Check if AudioManager instance exists
        if (AudioManager.instance == null)
        {
            Debug.LogError("AudioManager instance is missing! Ensure AudioManager is in the scene.");
            return;
        }

        // Initialize the slider with the current volume
        volumeSlider.value = AudioManager.instance.audioSource.volume;

        // Add a listener to update the volume when the slider value changes
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void OnVolumeChanged(float value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetVolume(value);
            AudioManager.instance.SaveVolume(); // Save the updated volume
        }
        else
        {
            Debug.LogError("AudioManager instance is missing! Cannot set volume.");
        }
    }
}
