using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;

    void Start()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);

        if (masterSlider != null)
        {
            masterSlider.value = master;
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (musicSlider != null)
        {
            musicSlider.value = music;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }
    }

    public void SetMasterVolume(float value)
    {
        if (GlobalAudioSettings.Instance != null)
            GlobalAudioSettings.Instance.SetMasterVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        if (GlobalAudioSettings.Instance != null)
            GlobalAudioSettings.Instance.SetMusicVolume(value);
    }
}