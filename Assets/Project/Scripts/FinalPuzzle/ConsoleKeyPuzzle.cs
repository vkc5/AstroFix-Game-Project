using System.Collections.Generic;
using UnityEngine;

public class ConsoleKeyPuzzle : MonoBehaviour
{
    public string correctWord = "REACT";
    public Timer timer;
    public float wrongPenalty = 20f;

    private bool playerNear = false;
    private PlayerKeyInventory inventory;

    private string currentInput = "";
    private List<int> usedIndexes = new List<int>();

    void Update()
    {
        if (!playerNear || inventory == null)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) InsertKey(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) InsertKey(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) InsertKey(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) InsertKey(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) InsertKey(4);
    }

    void InsertKey(int index)
    {
        if (!inventory.HasAllKeys())
        {
            Debug.Log("You need all 5 keys first.");
            return;
        }

        if (usedIndexes.Contains(index))
        {
            Debug.Log("This key already inserted.");
            return;
        }

        string letter = inventory.collectedKeys[index];
        currentInput += letter;
        usedIndexes.Add(index);

        Debug.Log("Inserted: " + letter);
        Debug.Log("Current input: " + currentInput);

        if (currentInput.Length == 5)
        {
            CheckAnswer();
        }
    }

    void CheckAnswer()
    {
        if (currentInput == correctWord)
        {
            Debug.Log("SUCCESS! ENGINE ACTIVATED");
        }
        else
        {
            Debug.Log("WRONG ORDER. Try again.");

            if (timer != null)
                timer.RemoveTime(wrongPenalty);

            ResetInput();
        }
    }

    void ResetInput()
    {
        currentInput = "";
        usedIndexes.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            inventory = other.GetComponent<PlayerKeyInventory>();

            Debug.Log("Console ready. Press keys 1-5 to insert.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            inventory = null;
            ResetInput();
        }
    }
}