using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioSettingsManager : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;

    [Range(0f, 1f)]
    public float sfxBaseVolume = 1f;

    private float masterVolume = 1f;
    private float musicVolume = 1f;

    void Start()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);

        if (masterSlider != null)
        {
            masterSlider.value = masterVolume;
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (musicSlider != null)
        {
            musicSlider.value = musicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        ApplyVolumes();
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        ApplyVolumes();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        ApplyVolumes();
    }

    public void ApplyVolumes()
    {
        AudioSource[] allSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource source in allSources)
        {
            if (source.CompareTag("Music"))
            {
                source.volume = masterVolume * musicVolume;
            }
            else if (source.CompareTag("SFX"))
            {
                source.volume = masterVolume * sfxBaseVolume;
            }
        }
    }
}