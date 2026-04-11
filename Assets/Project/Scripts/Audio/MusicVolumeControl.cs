using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeControl : MonoBehaviour
{
    public Slider slider;
    public AudioSource musicSource;

    private void Start()
    {
        slider.value = musicSource.volume;
        slider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }
}