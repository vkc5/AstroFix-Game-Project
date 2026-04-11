using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip clickSound;

    public void PlayClick()
    {
        if (sfxSource != null && clickSound != null)
        {
            sfxSource.PlayOneShot(clickSound);
        }
    }
}