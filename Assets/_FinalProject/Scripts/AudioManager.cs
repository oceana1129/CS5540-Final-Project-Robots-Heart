using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public GameObject btnAudio;         // button game object
    public Sprite iconAudioNone;         // icon for audio off
    public Sprite iconAudioMax;          // icon for audio on
    private AudioSource audioSource;    // AudioSource for the track
    private Image btnIcon;              // button current image
    private bool playAudio = false;     // toggle state for audio

    void Start()
    {
        btnIcon = btnAudio.GetComponent<Image>();                       // get image component
        audioSource = gameObject.GetComponent<AudioSource>();           // get audioSource component

        // check for required components
        if (audioSource == null) {                                      // if audioSource does not ecist
            Debug.LogError("Audio Source is not assigned");
            return;
        }
        
        if (btnAudio == null) {                                         // see if audio button exists
            Debug.LogError("Audio button is not assigned");
            return;
        }

        if (btnIcon == null) {                                          // see if audio button image component exists
            Debug.LogError("Image component not found on the button");
            return;
        }

        if (iconAudioNone == null || iconAudioMax == null) {            // see if audio icons were assigned
            Debug.LogError("Audio icons are not assigned");
            return;
        }

        btnIcon.sprite = iconAudioNone;                                 // initialize button with audio off
        Button button = btnAudio.GetComponent<Button>();                // find button from audio button component

        if (button != null) {                                           // if button exists
            button.onClick.AddListener(ToggleAudio);                        // add onClick listener to button
        }
        else {
            Debug.LogError("Button component not found on btnAudio");
        }
    }

    // toggle audio and icons
    public void ToggleAudio()
    {
        playAudio = !playAudio;                                     // toggle the playAudio state

        if (playAudio) {                                            // if play audio toggled
            audioSource.Play();                                         // play the audio
            btnIcon.sprite = iconAudioMax;                              // change button icon to audio on
        }
        else {                                                      // play audio not toggled
            audioSource.Pause();                                        // pause the audio
            btnIcon.sprite = iconAudioNone;                             // change button icon to audio off
        }
    }
}
