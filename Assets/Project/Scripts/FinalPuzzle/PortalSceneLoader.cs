using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PortalSceneLoader : MonoBehaviour
{
    public string endingSceneName = "EndingScene";

    [Header("Transition")]
    public Image flashPanel;
    public float flashDuration = 0.8f;

    private bool portalReady = false;
    private bool loading = false;

    public void ActivatePortal()
    {
        portalReady = true;
        Debug.Log("Portal is active. Enter to load ending scene.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!portalReady || loading) return;

        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            loading = true;
            StartCoroutine(FlashAndLoad());
        }
    }

    private IEnumerator FlashAndLoad()
    {
        if (flashPanel != null)
        {
            Color c = flashPanel.color;
            c.a = 0f;
            flashPanel.color = c;

            float timer = 0f;

            while (timer < flashDuration)
            {
                timer += Time.deltaTime;
                float t = timer / flashDuration;

                c.a = Mathf.Lerp(0f, 1f, t);
                flashPanel.color = c;

                yield return null;
            }
        }

        SceneManager.LoadScene(endingSceneName);
    }
}