using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeControl : MonoBehaviour
{
    public Slider slider;
    public AudioSource sfxSource;

    private void Start()
    {
        slider.value = sfxSource.volume;
        slider.onValueChanged.AddListener(SetMasterVolume);
    }

    public void SetMasterVolume(float value)
    {
        sfxSource.volume = value;
    }
}