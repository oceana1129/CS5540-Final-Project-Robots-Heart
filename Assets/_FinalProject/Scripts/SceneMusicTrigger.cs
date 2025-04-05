using UnityEngine;

public class SceneMusicTrigger : MonoBehaviour
{
    public AudioClip sceneClip; 

    void Start()
    {
        if (sceneClip != null && AudioManager.instance != null)
        {
            AudioManager.instance.ChangeTrack(sceneClip);
        }
    }
}
