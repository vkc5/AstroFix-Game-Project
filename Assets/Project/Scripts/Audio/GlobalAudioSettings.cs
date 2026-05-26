using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalAudioSettings : MonoBehaviour
{
    public static GlobalAudioSettings Instance;

    private float masterVolume;
    private float musicVolume;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        LoadSettings();
        ApplyVolumes();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadSettings();
        ApplyVolumes();
    }

    public void LoadSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    public void ApplyVolumes()
    {
        AudioSource[] allSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource source in allSources)
        {
            if (source.CompareTag("Music"))
                source.volume = masterVolume * musicVolume;
            else if (source.CompareTag("SFX"))
                source.volume = masterVolume;
        }
    }
}