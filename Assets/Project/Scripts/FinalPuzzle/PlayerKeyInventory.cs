using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerKeyInventory : MonoBehaviour
{
    public List<string> collectedKeys = new List<string>() { "R", "E", "A", "C" };

    public TextMeshProUGUI keyCounterText;
    public int totalKeys = 5;

    private void Start()
    {
        UpdateKeyUI();
    }

    public void AddKey(string letter)
    {
        Debug.Log("Trying to add key: " + letter);

        if (!collectedKeys.Contains(letter))
        {
            collectedKeys.Add(letter);
            Debug.Log("Key added. Total keys: " + collectedKeys.Count);
        }
        else
        {
            Debug.LogWarning("This key already exists: " + letter);
        }

        UpdateKeyUI();
    }

    public bool HasAllKeys()
    {
        return collectedKeys.Count >= totalKeys;
    }

    public string GetWord()
    {
        return string.Join("", collectedKeys);
    }

    private void UpdateKeyUI()
    {
        if (keyCounterText != null)
        {
            keyCounterText.text = collectedKeys.Count.ToString();
        }
        else
        {
            Debug.LogWarning("Key Counter Text is missing.");
        }
    }
}