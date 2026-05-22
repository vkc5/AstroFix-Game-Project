using System.Collections;
using UnityEngine;

public class LevelLightController : MonoBehaviour
{
    [Header("Lights")]
    public Light[] roomLights;
    public float targetIntensity = 2f;
    public float fadeSpeed = 1f;

    [Header("Monitors")]
    public GameObject[] monitorErrors;
    public GameObject[] monitorNormals;

    private bool activated;


    public AudioSource victoryAudio;
    void Start()
    {
        foreach (Light l in roomLights)
        {
            if (l != null)
                l.intensity = 0f;
        }

        foreach (GameObject obj in monitorErrors)
            if (obj != null) obj.SetActive(true);

        foreach (GameObject obj in monitorNormals)
            if (obj != null) obj.SetActive(false);
    }

    public void TurnOnLevel()
    {
        if (activated) return;
        activated = true;

        foreach (GameObject obj in monitorErrors)
            if (obj != null) obj.SetActive(false);

        foreach (GameObject obj in monitorNormals)
            if (obj != null) obj.SetActive(true);
        if (victoryAudio != null)
            victoryAudio.Play();
        FindFirstObjectByType<LevelStateManager>().CompleteLevel();
        FindFirstObjectByType<LevelCompletePopup>().ShowLevelComplete();
        StartCoroutine(FadeLightsOn());
    }

    IEnumerator FadeLightsOn()
    {
        float current = 0f;

        while (current < targetIntensity)
        {
            current += Time.deltaTime * fadeSpeed;

            foreach (Light l in roomLights)
                if (l != null) l.intensity = current;

            yield return null;
        }
    }
}