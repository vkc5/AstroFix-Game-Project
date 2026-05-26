using UnityEngine;
using System.Collections;

public class KeyItem : MonoBehaviour
{
    public string letter = "T";
    public GameObject interactText;

    [Header("Collect Animation")]
    public Transform collectTarget;
    public float moveDuration = 0.45f;

    [Header("Final Hand Position")]
    public Vector3 handLocalPosition = Vector3.zero;
    public Vector3 handLocalRotation = new Vector3(90f, 0f, 90f);
    public Vector3 handLocalScale = new Vector3(0.0001f, 0.0001f, 0.0001f);

    private bool playerNear = false;
    private bool collected = false;
    private PlayerKeyInventory playerInventory;
    private Collider keyCollider;

    private void Start()
    {
        keyCollider = GetComponent<Collider>();

        if (interactText != null)
            interactText.SetActive(false);
    }

    private void Update()
    {
        if (playerNear && !collected && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory == null)
            {
                Debug.LogWarning("Player inventory missing.");
                return;
            }

            collected = true;

            playerInventory.AddKey(letter);

            if (interactText != null)
                interactText.SetActive(false);

            gameObject.SetActive(false);
            return;

        }
    }

    private IEnumerator AnimateToHand()
    {
        if (collectTarget == null)
        {
            Debug.LogWarning("Collect Target is missing.");
            yield break;
        }

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        Vector3 endPosition = collectTarget.position;
        Quaternion endRotation = collectTarget.rotation;

        float timer = 0f;

        while (timer < moveDuration)
        {
            timer += Time.deltaTime;
            float t = timer / moveDuration;

            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);

            yield return null;
        }

        // Attach the real key to the hand/socket after the animation finishes
        transform.SetParent(collectTarget, false);

        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);

        Debug.Log("FINAL KEY ROTATION: " + transform.localEulerAngles);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            playerNear = true;

            playerInventory = other.GetComponent<PlayerKeyInventory>();

            if (playerInventory == null)
                playerInventory = other.transform.root.GetComponent<PlayerKeyInventory>();

            if (interactText != null)
                interactText.SetActive(true);

            Debug.Log("Player near final key. Press E.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            playerNear = false;

            if (interactText != null)
                interactText.SetActive(false);
        }
    }
}