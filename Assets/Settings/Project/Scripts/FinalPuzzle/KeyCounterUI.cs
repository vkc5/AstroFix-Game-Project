using TMPro;
using UnityEngine;
using System.Collections;

public class KeyCounterUI : MonoBehaviour
{
    [Header("Key Settings")]
    public int currentKeys = 4;
    public int maxKeys = 5;

    [Header("UI")]
    public TMP_Text keyCountText;
    public GameObject allKeysText;

    [Header("Message Settings")]
    public float messageDuration = 1f;

    private void Start()
    {
        if (allKeysText != null)
            allKeysText.SetActive(false);

        UpdateKeyUI();
    }

    public void AddKey()
    {
        currentKeys++;

        if (currentKeys > maxKeys)
            currentKeys = maxKeys;

        UpdateKeyUI();
    }

    public void SetKeyCount(int amount)
    {
        currentKeys = amount;

        if (currentKeys > maxKeys)
            currentKeys = maxKeys;

        UpdateKeyUI();
    }

    private void UpdateKeyUI()
    {
        if (keyCountText != null)
            keyCountText.text = currentKeys.ToString();

        if (currentKeys >= maxKeys)
            StartCoroutine(ShowAllKeysMessage());
    }

    private IEnumerator ShowAllKeysMessage()
    {
        if (allKeysText != null)
        {
            allKeysText.SetActive(true);
            yield return new WaitForSeconds(messageDuration);
            allKeysText.SetActive(false);
        }
    }
}