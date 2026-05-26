using UnityEngine;

public class PowerGlowManager : MonoBehaviour
{
    public Renderer[] glowRenderers;
    public Color glowColor = Color.cyan;
    public float glowIntensity = 4f;

    void Start()
    {
        foreach (Renderer r in glowRenderers)
        {
            if (r != null)
            {
                r.material.DisableKeyword("_EMISSION");
                r.material.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    public void TurnOnGlow()
    {
        foreach (Renderer r in glowRenderers)
        {
            if (r != null)
            {
                r.material.EnableKeyword("_EMISSION");
                r.material.SetColor("_EmissionColor", glowColor * glowIntensity);
            }
        }
    }
}