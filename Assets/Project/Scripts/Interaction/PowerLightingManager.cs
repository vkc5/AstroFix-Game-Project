using UnityEngine;

public class PowerLightingManager : MonoBehaviour
{
    public GameObject[] powerLightObjects;

    void Start()
    {
        foreach (GameObject obj in powerLightObjects)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    public void TurnOnPowerLights()
    {
        foreach (GameObject obj in powerLightObjects)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }
}