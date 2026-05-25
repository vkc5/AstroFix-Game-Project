using UnityEngine;

public class PowerRoomAudioManager : MonoBehaviour
{
    public AudioSource backgroundAudio;

    void Start()
    {
        if (backgroundAudio != null)
        {
            backgroundAudio.loop = true;
            backgroundAudio.playOnAwake = true;
            backgroundAudio.spatialBlend = 0f;
            backgroundAudio.volume = 0.3f;
            backgroundAudio.Play();
        }
    }
}