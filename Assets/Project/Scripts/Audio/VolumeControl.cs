using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider slider;

    private void Start()
    {
        if (slider != null)
            slider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        if (audioSource != null)
            audioSource.volume = value;
    }
}